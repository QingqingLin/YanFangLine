using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Linq;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace MonitorPorts
{
    public partial class MainForm : Form
    {
        CatchSocket Catch;
        BindNetworkCard Bind;
        FilterForm FilterOptionForm;
        bool Pause = true;
        public delegate void UpdateForm(PacketProperties Properties);
        Dictionary<string, Color> DicColor = new Dictionary<string, Color>();
        DateTime countInterval;
        List<string> xData = new List<string> { "0-500", "500-1000", "1000-2400", "≥2400" };
        List<int> yData = new List<int> { 0, 0, 0, 0 };
        List<string> VOBCToZC = new List<string>();
        List<string> VOBCToCI = new List<string>();
        List<string> VOBCToATS = new List<string>();
        List<string> ZCToVOBC = new List<string>();
        List<string> CIToVOBC = new List<string>();
        List<string> ATSToVOBC = new List<string>();
        private bool DeleteFlag = true;
        public static ulong NumOfReceivePackets = 0;
        public static ulong NumOfFilterPackets = 0;
        public static bool IsFirstClick;
        public static System.Threading.Timer CreatPcapPerDay;
        public static System.Threading.Timer OutputDataTimer;
        public static System.Threading.Timer ShowTimer;

        public static List<PacketProperties> TemporaryStorage1 = new List<PacketProperties>();
        public static List<PacketProperties> TemporaryStorage2 = new List<PacketProperties>();
        Dictionary<string, DateTime> DicInterval = new Dictionary<string, DateTime>();


        #region//守护进程所用变量

        public Communication comm;
        public Thread thSend;
        public Thread thCheck;
        public Thread thReceive;
        #endregion


        public MainForm()
        {
            InitializeComponent();
            WriteProgramLog("打开软件");
            Catch = new CatchSocket(this);
            Catch.CreatPcapFile();                      
            FilterOptionForm = new FilterForm();

            DateTime dt1 = DateTime.Parse(DateTime.Now.ToShortDateString() + " 23:59:59");
            DateTime dt2 = DateTime.Now;
            TimeSpan ts = new TimeSpan();
            ts = dt1 - dt2;
            Int64 IntervalToTomorrow0 = Convert.ToInt64(ts.TotalMilliseconds + 1800000);
            CreatPcapPerDay = new System.Threading.Timer(new TimerCallback(Catch.CreatPcapFilePerDay), null, IntervalToTomorrow0, IntervalToTomorrow0);
            OutputDataTimer = new System.Threading.Timer(new TimerCallback(Catch.OutToPcap),null,-1,0);
            ShowTimer = new System.Threading.Timer(new TimerCallback(ShowThread.Show), null, -1, 0);

            //守护进程使用，实例化Communication类
            comm = new Communication();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            IsFirstClick = true;
            this.Button_Start.Enabled = false;
            this.Button_Stop.Enabled = false;
            this.Button_Pause.Enabled = false;
            ShowThread.PacketArrival += ShowInForm;
            Thread innerDelFileThread = new Thread(DelFileThread);
            innerDelFileThread.IsBackground = true;
            innerDelFileThread.Start();
            Bind = new BindNetworkCard();

            LoadHistory();


            //守护程序用 开线程发送和接收消息，并检测通讯状况
            thSend = new Thread(comm.Send);
            thSend.Start();
            thSend.IsBackground = true;

            thCheck = new Thread(comm.Check);
            thCheck.Start();
            thCheck.IsBackground = true;

            thReceive = new Thread(comm.ReceiveMessages);
            thReceive.Start();
            thReceive.IsBackground = true;

            comm.Restart();


        }

        private void DelFileThread()
        {
            string pathConfigurationInfo = Application.StartupPath + @"\DeleteDataInterval.txt";
            StreamReader objReader = new StreamReader(pathConfigurationInfo, Encoding.GetEncoding("gb2312"));
            string interval = objReader.ReadLine();
            ushort DeleteDataInterval = Convert.ToUInt16(interval);
            DeleteData Delete = new DeleteData(DeleteDataInterval);
            while (DeleteFlag)
            {
                Delete.DeleteOverdueData();
                Thread.Sleep(60 * 60 * 1000);
            }
        }

        private void Button_Filter_Click(object sender, EventArgs e)
        {
            if (FilterOptionForm.ShowDialog() == DialogResult.OK)
            {
                FilterOptionForm.Show();
            }
            this.Button_Start.Enabled = true;
        }

        private void Button_Start_Click(object sender, EventArgs e)
        {
            WriteProgramLog("开始监测");
            countInterval = DateTime.Now;
            this.Button_Start.Enabled = false;
            this.Button_Stop.Enabled = true;
            this.Button_Pause.Enabled = true;
            this.Button_Filter.Enabled = false;
            if (Pause)
            {
                Pause = false;
                Catch.Start(Bind.socket);
            }
            else
            {
                Clear();
                Catch.Start(Bind.socket);
            }
            ServiceStatus.Text = "开始监听";



            #region 守护程序用 开线程发送和接收消息，并检测通讯状况
            if (thSend == null || thSend.IsAlive == false)
            {
                thSend = new Thread(comm.Send);
                thSend.IsBackground = true;
                thSend.Start();                
            }

            if (thCheck == null || thCheck.IsAlive == false)
            {
                thCheck = new Thread(comm.Check);
                thCheck.IsBackground = true;
                thCheck.Start();             
            }

            if(thReceive==null||thReceive.IsAlive==false)
            {
                thReceive = new Thread(comm.ReceiveMessages);
                thReceive.IsBackground = true;
                thReceive.Start();
            }

            comm.Restart();

            #endregion
        }

        private void Button_Stop_Click(object sender, EventArgs e)
        {
            WriteProgramLog("停止监测");
            Catch.Stop();
            this.Button_Start.Enabled = true;
            this.Button_Filter.Enabled = true;
            this.Button_Pause.Enabled = false;
            this.Button_Stop.Enabled = false;
            ServiceStatus.Text = "停止监听";

            //if (thSend.IsAlive)
            //{
            //    thSend.Abort();
            //}

        }

        private void Button_Pause_Click(object sender, EventArgs e)
        {
            WriteProgramLog("暂停监测");
            Catch.Stop();
            Pause = true;
            this.Button_Start.Enabled = true;
            this.Button_Filter.Enabled = true;
            this.Button_Pause.Enabled = false;
            this.Button_Stop.Enabled = false;
            ServiceStatus.Text = "暂停监听";
        }

        public void ShowInForm(PacketProperties Properties)
        {
            if (this.IsHandleCreated)
            {
                this.BeginInvoke(new UpdateForm(Show), Properties);
            }
        }

        private void Show(PacketProperties Properties)
        {
            switch (Properties.SendDir)
            {
                case SendDir.VOBCToZC:
                    UpdateChart(FilterOptionForm.MaxVOBCToZC, Properties.OriginationAddress + "(" + Properties.OriginationPort + ")" + "-ZC", this.listViewVOBCToZC, this.chartVOBCtoZCLine, this.chartVOBCtoZCPie, VOBCToZC, Properties);
                    break;
                case SendDir.VOBCToCI:
                    UpdateChart(FilterOptionForm.MaxVOBCToCI, Properties.OriginationAddress + "(" + Properties.OriginationPort + ")" + "-CI", this.listViewVOBCToCI, this.chartVOBCToCILine, this.chartVOBCtoCIPie, VOBCToCI, Properties);
                    break;
                case SendDir.VOBCToATS:
                    UpdateChart(FilterOptionForm.MaxVOBCToATS, Properties.OriginationAddress + "(" + Properties.OriginationPort + ")" + "-ATS", this.listViewVOBCToATS, this.chartVOBCToATSLine, this.chartVOBCToATSPie, VOBCToATS, Properties);
                    break;
                case SendDir.ZCToVOBC:
                    UpdateChart(FilterOptionForm.MaxZCToVOBC, "ZC-" + Properties.DestinationAddress + "(" + Properties.DestinationPort + ")", this.listViewZCToVOBC, this.chartZCtoVOBCLine, this.chartZCtoVOBCPie, ZCToVOBC, Properties);
                    break;
                case SendDir.CIToVOBC:
                    UpdateChart(FilterOptionForm.MaxCIToVOBC, "CI-" + Properties.DestinationAddress + "(" + Properties.DestinationPort + ")", this.listViewCIToVOBC, this.chartCIToVOBCLine, this.chartCIToVOBCPie, CIToVOBC, Properties);
                    break;
                case SendDir.ATSToVOBC:
                    UpdateChart(FilterOptionForm.MaxATSToVOBC, "ATS-" + Properties.DestinationAddress + "(" + Properties.DestinationPort + ")", this.listViewATSToVOBC, this.chartATSToVOBCLine, this.chartATSToVOBCPie, ATSToVOBC, Properties);
                    break;
            }
        }

        private void UpdateChart(double AlarmInterval, string Name, ListView listView_Data, Chart chartLine, Chart chartPie, List<string> NameSeries, PacketProperties Properties)
        {
            if (DicInterval.Keys.Contains(Name))
            {
                Properties.Interval = (Properties.CaptureTime - DicInterval[Name]).TotalSeconds;
                DicInterval[Name] = Properties.CaptureTime;
            }
            else
            {
                Properties.Interval = 0;
                DicInterval.Add(Name, Properties.CaptureTime);
            }
            if (Convert.ToDouble(Properties.Interval) > AlarmInterval)
            {
                string MessageBodyHex = GetDataHex(Properties.MessageBuffer, 8, (int)Properties.MessageLength - 8);
                listView_Data.Items.Add(new ListViewItem(new string[] { Properties.CaptureTime.ToString("yyyy-MM-dd"), Properties.CaptureTime.ToString("HH:mm:ss:fff"), Properties.Interval.ToString(), Properties.OriginationAddress, Properties.DestinationAddress, MessageBodyHex }));
                SaveToAlarmLog(AlarmInterval, Properties, MessageBodyHex);
            }
            int indexOfSeries;
            if (NameSeries.Contains(Name))
            {
                indexOfSeries = NameSeries.IndexOf(Name);
            }
            else
            {
                NameSeries.Add(Name);
                DicColor.Add(Name, GetRandomColor());
                Series s1 = new Series();
                s1.Name = Name;
                chartLine.Series.Add(s1);
                indexOfSeries = NameSeries.IndexOf(Name);
            }
            if (chartLine.Series[indexOfSeries].Points.Count() >= 1500)
            {
                chartLine.Series[indexOfSeries].Points.Remove(chartLine.Series[indexOfSeries].Points[0]);
            }
            double Interval;
            Interval = Properties.Interval;
            if (Properties.Interval > 6)
            {
                Interval = 6;
            }
            chartLine.Series[indexOfSeries].Points.AddY(Interval);

            chartLine.Series[indexOfSeries].ChartType = SeriesChartType.Line;
            chartLine.Series[indexOfSeries].BorderWidth = 3;
            chartLine.Series[indexOfSeries].Color = DicColor[Name];

            double interval = Convert.ToDouble(Properties.Interval) * 1000;
            if (interval >= 0 && interval < 500) yData[0]++;
            else if (interval >= 500 && interval < 1000) yData[1]++;
            else if (interval >= 1000 && interval < 2400) yData[2]++;
            else yData[3]++;
            if ((DateTime.Now - countInterval).TotalMilliseconds >= 10000)
            {
                chartPie.Series[0]["PieLabelStyle"] = "Outside";
                chartPie.Series[0]["PieLineColor"] = "Black";
                chartPie.Series[0].Points.DataBindXY(xData, yData);
            }
            //Thread.Sleep(0);
        }

        private void SaveToAlarmLog(double Interval, PacketProperties Properties, string MessageBodyHex)
        {
            string path = "AlarmLog";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string Head = Properties.CaptureTime + ",";
            Head += Properties.Interval + ",";
            Head += Properties.OriginationAddress + "," + Properties.OriginationPort + "," + Properties.DestinationAddress + "," + Properties.DestinationPort + ",";
            Head += MessageBodyHex.Replace("\r\n", string.Empty);
            path += "\\" + Properties.CaptureTime.Year + "-" + Properties.CaptureTime.Month + "-" + Properties.CaptureTime.Day + ".csv";
            if (File.Exists(path))
            {
                using (StreamWriter file = new StreamWriter(path, true, Encoding.UTF8))
                {
                    file.WriteLine(Head);
                    file.Close();
                }
            }
            else
            {
                using (StreamWriter file = new StreamWriter(path, true, Encoding.UTF8))
                {
                    file.WriteLine("捕获时间,间隔,源IP,源端口,目的IP,目的端口,消息内容");
                    file.WriteLine(Head);
                    file.Close();
                }
            }
        }

        private void LoadHistory()
        {
            int PcapLength;
            string PcapLengthUnit;
            List<string> BackUps = new List<string>();
            StreamReader sr = new StreamReader("BackUps.txt", Encoding.UTF8);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                BackUps.Add(line);
            }
            sr.Close();
            if (BackUps.Count != 0)
            {
                try
                {
                    PcapLength = Convert.ToInt16(BackUps[0]);
                    PcapLengthUnit = BackUps[1];
                    if (PcapLengthUnit == "MB")
                    {
                        FilterForm.PcapLengthNum = PcapLength * 1024 * 1024;
                    }
                    if (PcapLengthUnit == "GB")
                    {
                        FilterForm.PcapLengthNum = PcapLength * 1024 * 1024 * 1024;
                    }

                    FilterOptionForm.MaxVOBCToZC = Convert.ToDouble(BackUps[4]) / 1000;
                    FilterOptionForm.MaxZCToVOBC = Convert.ToDouble(BackUps[5]) / 1000;
                    FilterOptionForm.MaxVOBCToCI = Convert.ToDouble(BackUps[6]) / 1000;
                    FilterOptionForm.MaxCIToVOBC = Convert.ToDouble(BackUps[7]) / 1000;
                    FilterOptionForm.MaxVOBCToATS = Convert.ToDouble(BackUps[8]) / 1000;
                    FilterOptionForm.MaxATSToVOBC = Convert.ToDouble(BackUps[9]) / 1000;

                    string[] SourList = BackUps[2].Substring(1).Split(new char[] { '#' });
                    for (int i = 0; i < SourList.Length; i++)
                    {
                        foreach (var item in IPList.ConfigProperties)
                        {
                            if (item.Name == SourList[i])
                            {
                                item.IsSourceChoose = true;
                            }
                        }
                    }
                    string[] DestList = BackUps[3].Substring(1).Split(new char[] { '#' });
                    for (int i = 0; i < DestList.Length; i++)
                    {
                        foreach (var item in IPList.ConfigProperties)
                        {
                            if (item.Name == DestList[i])
                            {
                                item.IsDestChoose = true;
                            }
                        }
                    }

                    countInterval = DateTime.Now;
                    this.Button_Start.Enabled = false;
                    this.Button_Stop.Enabled = true;
                    this.Button_Pause.Enabled = true;
                    this.Button_Filter.Enabled = false;
                    if (Pause)
                    {
                        Pause = false;
                        Catch.Start(Bind.socket);
                    }
                    else
                    {
                        Clear();
                        Catch.Start(Bind.socket);
                    }
                    ServiceStatus.Text = "开始监听";
                }
                catch (Exception e)
                {

                }
            }
        }

        private string GetDataHex(Byte[] Data, int index, int count)
        {
            string DataHex = "";
            for (int i = index; i < index + count; i++)
            {
                if (i > index && (i - index) % 16 == 0)
                {
                    DataHex += "\r\n";
                }
                if (Data[i].ToString("X").Length != 1)
                {
                    DataHex += Data[i].ToString("X") + " ";
                }
                else
                {
                    DataHex += "0" + Data[i].ToString("X") + " ";
                }
            }

            return DataHex;
        }

        private System.Drawing.Color GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            int int_Red = RandomNum_First.Next(256);
            int int_Green = RandomNum_Sencond.Next(256);
            int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            return System.Drawing.Color.FromArgb(int_Red, int_Green, int_Blue);
        }

        private void Button_ClearList_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            NumOfFilterPackets = 0;
            NumOfReceivePackets = 0;
            listViewVOBCToZC.Items.Clear();
            listViewVOBCToATS.Items.Clear();
            listViewVOBCToCI.Items.Clear();
            listViewZCToVOBC.Items.Clear();
            listViewCIToVOBC.Items.Clear();
            listViewATSToVOBC.Items.Clear();
            Chart[] charts = { chartVOBCToATSLine, chartVOBCToATSPie, chartVOBCToCILine, chartVOBCtoCIPie, chartVOBCtoZCLine, chartVOBCtoZCPie,
                               chartZCtoVOBCLine, chartZCtoVOBCPie, chartCIToVOBCLine, chartCIToVOBCPie, chartATSToVOBCLine, chartATSToVOBCPie};
            ClearChart(charts);
        }

        private void ClearChart(Chart[] charts)
        {
            foreach (var item in charts)
            {
                for (int i = 0; i < item.Series.Count(); i++)
                {
                    item.Series[i].Points.Clear();
                }
            }
        }

        private void Buton_Find_Click(object sender, EventArgs e)
        {
            QueryForm queryFrm = new QueryForm(this);
            queryFrm.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("你确定要退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                //守护程序使用
                if (thSend != null && thSend.IsAlive)
                {
                    thSend.Abort();
                }
                if (thCheck != null && thCheck.IsAlive)
                {
                    thCheck.Abort();
                }

                comm.SendFiveTimes();

                GeneratePcapFile.fs.Flush();
                GeneratePcapFile.fs.Close();

                WriteProgramLog("正常退出软件");
                System.Environment.Exit(0);
                this.Dispose();
            }
        }

        private void WriteProgramLog(string logs)
        {
            string LogAddress = Environment.CurrentDirectory + "\\Log.log";
            StreamWriter sw = new StreamWriter(LogAddress, true);
            sw.WriteLine(string.Format("[{0}] {1}", DateTime.Now.ToString(), logs));
            sw.Flush();
            sw.Close();
        }
    }
}