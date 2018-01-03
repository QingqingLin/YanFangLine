using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Management;
using System.Net.Sockets;
using System.Threading;

using Excel = Microsoft.Office.Interop.Excel;
using System.Globalization;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using UnpackPcap;
using System.Runtime.InteropServices;

namespace MonitorPorts
{
    public partial class QueryForm : Form
    {
        #region 全局变量
        private int ItemID;
        private double captureTime;
        private int FilterCount;
        DateTime startTime;
        delegate void listViewDelegate(string ID, string Capturetime, string Protocol, string SourceIP, string SourcePort, string DestIP, string DestPort, string AllLength, string MessageBodyTxt, string MessageBodyLen, string MessageBodyHex);
        Dictionary<string, string> dic;
        MainForm main;
        #endregion
        public QueryForm(MainForm main)
        {
            InitializeComponent();
            this.main = main;
            ItemID = 0;
            FilterCount = 0;
            //dic = ReadIPlist();
            //ShowInForm(dic);

        }
        //筛选符合条件pcap文件
        private void Button_Filter_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Clear();
            DateTime startData = startDate.Value.Date;                                                  // dateTimePickerStart.Value.Date;//开始日期

            DateTime endData = endDate.Value.Date;                                                      //终止日期

            DateTime _startTime = startData + new TimeSpan(startHour.Value.Hour, startHour.Value.Minute, startHour.Value.Second);//开始时间

            DateTime _endTime = endData + new TimeSpan(endHour.Value.Hour, endHour.Value.Minute, endHour.Value.Second); ;//终止时间

            int frameID = 1;                                                                        // 数据帧序号记录
            FilterPcap filter = new FilterPcap();                                                    //实例化一个根据文件时间筛选数据帧的类
            List<string> fileNameList = filter.getReqrFileList(_startTime, _endTime, Application.StartupPath + "\\pcap");      //获得所有满足条件的文件的文件名

            if (fileNameList.Count == 0)      //不符合时间筛选条件的处理
            {
                MessageBox.Show("没有找到指定的数据信息，请重新配置筛选条件！");    //时间查找不符合
                this.Cursor = Cursors.Default;
                return;
            }
            else  //符合时间筛选条件的处理
            {
                //选取筛选的IP

                string sourIP = textBox1.Text;
                string destIP = textBox2.Text;
                string srcPort = textBox3.Text;
                string dstPort = textBox4.Text;
                DateTime preTime = new DateTime(), nowTime = new DateTime();             //   统计相邻两个数据帧时间间隔

                // 符合文件的遍历
                foreach (string filepath in fileNameList)
                {
                    try
                    {
                        PacapReader reader = new PacapReader(filepath);
                        List<PacapData> dataList = reader.Parse();
                        reader.Close();
                        #region
                        for (int i = 0; i < dataList.Count; i++)
                        {
                            string[] frame = new string[11];                    //列表行内容
                            frame[4] = dataList[i].Source.Address.ToString();                      // 第五列  源IP地址
                            frame[6] = dataList[i].Dest.Address.ToString();                  // 第七列  目的IP地址
                            int[] Ports = dataList[i].getPort();
                            frame[5] = Ports[0].ToString();                   // 第六列  源端口号
                            frame[7] = Ports[1].ToString();                 // 第八列  目的端口号
                            if ((frame[4] == sourIP) && (frame[6] == destIP) && frame[5] == srcPort && frame[7] == dstPort)   //符合IP筛选条件的处理
                            {
                                frame[0] = frameID.ToString();                      //第一列 帧序号
                                DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(dataList[i].PacketHeader.Ts.Timestamp_S).AddMilliseconds(dataList[i].PacketHeader.Ts.Timestamp_MS / 1000);
                                frame[1] = dt.ToString("yyyy-MM-dd HH:mm:ss fff");  // 第二列 帧时间
                                                                                    //  时间间隔处理
                                if (frameID == 1)
                                {
                                    preTime = dt;
                                    nowTime = dt;
                                }
                                else
                                {
                                    preTime = nowTime;
                                    nowTime = dt;
                                }

                                if ((nowTime - preTime).TotalSeconds > 60)                  //两个不同文件的处理
                                {
                                    frame[2] = Convert.ToString(6);
                                }
                                else
                                {
                                    frame[2] = (nowTime - preTime).TotalSeconds.ToString();     //第三列  时间间隔

                                }
                                frame[3] = dataList[i].ProtocolType.ToString();// 第四列  协议类型
                                frame[8] = dataList[i].Data.Length.ToString();  //  第九列 消息长度
                                frame[9] = dataList[i].Data.Length.ToString();
                                //foreach (int a in dataList[i].Data)                            //第十列 数据帧长度
                                //{
                                //    string b = a.ToString("X2");                              //数据帧内容转换为两位16进制大写
                                //    frame[10] = frame[10] + b + " ";                            //第十一列  数据帧内容
                                //}
                                //frame[10] = System.Text.Encoding.Default.GetString(dataList[i].Data);

                                frameID++;                               //数据帧序号加1
                                Frame.Add(frame);
                            }
                        }
                        #endregion

                    }
                    catch (Exception)
                    {
                    }
                    

                }

            }

            if (Frame.Count == 0)  //不符合IP筛选的处理
            {
                MessageBox.Show("没有找到指定的数据信息，请重新配置筛选条件！");
                this.Cursor = Cursors.Default;
                Frame.Clear();     //筛选出的数据集合清空，合理性待考虑
                return;
            }
            else
            {
                plot = new Thread(Plot);
                plot.Start();
                dtInfo = createDt();
                labLoad.Text = "数据加载完成！";
                Frame.Clear();
                InitDataSet();
                下一页.Enabled = true;
                上一页.Enabled = true;
                关闭.Enabled = true;
                go.Enabled = true;
                lblPageCount.Enabled = true;

            }
        }
        private void QueryForm_Load(object sender, EventArgs e)
        {

        }
        Thread plot;
        List<string[]> Frame = new List<string[]>();

        private void Plot()
        {
            #region 统计画图
            this.Invoke(new EventHandler(delegate
            {
                Series sb = new Series();
                sb.Name = "数据包接收间隔";
                Chart_Offline.Series.Add(sb);
                this.Chart_Offline.Series[0].ChartType = SeriesChartType.Line;
                Chart_Offline.Series[0].BorderWidth = 1;
                Chart_Offline.Series[0].Color = Color.Blue;
                Chart_Offline.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                Chart_Offline.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = double.NaN;
                Chart_Offline.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 1;
                Chart_Offline.ChartAreas[0].AxisX.Title = "包个数";
                Chart_Offline.ChartAreas[0].AxisY.Title = "时间（单位：秒）";
                foreach (DataRow s in dtInfo.Rows)
                {
                    this.Chart_Offline.Series[0].Points.AddY(Convert.ToDouble(s.ItemArray[2]));
                }
                this.Cursor = Cursors.Default;
            }
            ));
            #endregion
        }

        private void Button_OutputExcel_Click(object sender, EventArgs e)
        {
            ExportToExecl();
        }

        private void Button_Clear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void listView_Data_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            //TextBox_Hex.Text = e.Item.SubItems[10].Text;
        }

        #region 导出到Excel
        private void ExportToExecl()
        {
            saveFileDialog1.Filter = "Excel文件|*.xls|所有文件|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!System.String.IsNullOrEmpty(saveFileDialog1.FileName))
                {
                    string ExcelFileName = saveFileDialog1.FileName;
                    DoExport(ExcelFileName);
                }
            }
        }

        private void DoExport(string excelFileName)
        {
            int rowNum = listView_Data.Items.Count;
            int columnNum = listView_Data.Columns.Count;
            int rowIndex = 1;
            int columnIndex = 0;
            if (rowNum == 0 || string.IsNullOrEmpty(excelFileName))
            {
                return;
            }
            if (rowNum > 0)
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                if (xlApp == null)
                {
                    MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel");
                    return;
                }
                xlApp.DefaultFilePath = "";
                xlApp.DisplayAlerts = true;
                xlApp.SheetsInNewWorkbook = 1;
                Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);
                //将ListView的列名导入Excel表第一行
                foreach (ColumnHeader dc in listView_Data.Columns)
                {
                    columnIndex++;
                    xlApp.Cells[rowIndex, columnIndex] = dc.Text;
                }
                //将ListView中的数据导入Excel中
                for (int i = 0; i < rowNum; i++)
                {
                    rowIndex++;
                    columnIndex = 0;
                    for (int j = 0; j < columnNum; j++)
                    {
                        columnIndex++;
                        xlApp.Cells[rowIndex, columnIndex] = Convert.ToString(listView_Data.Items[i].SubItems[j].Text) + "\t";
                    }
                }
                xlBook.SaveAs(excelFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                xlApp = null;
                xlBook = null;
                MessageBox.Show("OK");
            }
        }
        #endregion

        private void Clear()
        {
            Frame.Clear();   //清除数据帧
            listView_Data.Items.Clear();   //清除列表
            dtInfo.Clear();
            Chart_Offline.Series.Clear();
            下一页.Enabled = false;
            上一页.Enabled = false;
            关闭.Enabled = false;
            go.Enabled = false;
            lblPageCount.Enabled = false;
            lblPageCount.Text = "/ 0";
            txtCurrentPage.Text = "0";
            labLoad.Text = "";

        }


        #region 加载IPlist并显示到两个listbox中
        private Dictionary<string, string> ReadIPlist()
        {
            Dictionary<string, string> tmpdic = new Dictionary<string, string>();
            List<string> list = new List<string>();
            string path = "IPlist.csv";
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                list.Add(line);
            }
            for (int i = 1; i < list.Count; i++)
            {
                string[] strs = list[i].Split(new char[] { ',' });
                string IP = strs[1];
                tmpdic.Add(strs[0], IP);
            }
            return tmpdic;
        }

        private void QueryForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            main.Show();
        }


        //private void ShowInForm(Dictionary<string, string> dic)
        //{
        //    foreach (string str in dic.Keys)
        //    {
        //        SourceListBox1.Items.Add(str);
        //        DestListBox2.Items.Add(str);
        //    }
        //}
        #endregion

        int pageSize = 0;     //每页显示行数
        int nMax = 0;         //总记录数
        int pageCount = 0;    //页数＝总记录数/每页显示行数
        int pageCurrent = 0;   //当前页号
        int nCurrent = 0;      //当前记录行

        DataTable dtInfo = new DataTable();
        private void InitDataSet()
        {

            pageSize = 1000;      //设置页面行数
            nMax = dtInfo.Rows.Count;
            pageCount = (nMax / pageSize);    //计算出总页数
            if ((nMax % pageSize) > 0) pageCount++;
            pageCurrent = 1;    //当前页数从1开始
            nCurrent = 0;       //当前记录数从0开始
            LoadData();
        }
        private void LoadData()
        {
            int nStartPos = 0;   //当前页面开始记录行
            int nEndPos = 0;     //当前页面结束记录行  
            DataTable dtTemp = dtInfo.Clone();   //克隆DataTable结构框架
            if (pageCurrent == pageCount)
                nEndPos = nMax;
            else
                nEndPos = pageSize * pageCurrent;
            nStartPos = nCurrent;

            //lblPageCount2.Text = pageCount.ToString();
            txtCurrentPage.Text = Convert.ToString(pageCurrent);
            lblPageCount.Text = "/ " + pageCount.ToString();
            this.listView_Data.Items.Clear();
            //从元数据源复制记录行
            for (int i = nStartPos; i < nEndPos; i++)
            {
                dtTemp.ImportRow(dtInfo.Rows[i]);

                //dtInfo.Rows[i].ItemArray[0]
                //14
                //dtInfo.Rows[i].ItemArray[2]
                //"在线对应参数设置"

                var lvi = new ListViewItem(new string[] { dtInfo.Rows[i].ItemArray[0].ToString(),dtInfo.Rows[i].ItemArray[1].ToString(),
                    dtInfo.Rows[i].ItemArray[2].ToString(),dtInfo.Rows[i].ItemArray[3].ToString(),dtInfo.Rows[i].ItemArray[4].ToString(),
                     dtInfo.Rows[i].ItemArray[5].ToString(),dtInfo.Rows[i].ItemArray[6].ToString(),dtInfo.Rows[i].ItemArray[7].ToString(),
                      dtInfo.Rows[i].ItemArray[8].ToString(),
                        });

                listView_Data.Items.Add(lvi);
                nCurrent++;
            }
            bdsInfo.DataSource = dtTemp;
            bdnInfo.BindingSource = bdsInfo;
            //listView1.DataSource = bdsInfo;
        }

        private DataTable createDt()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("name1", typeof(string));
            dt.Columns.Add("name2", typeof(string));
            dt.Columns.Add("name3", typeof(string));
            dt.Columns.Add("name4", typeof(string));
            dt.Columns.Add("name5", typeof(string));
            dt.Columns.Add("name6", typeof(string));
            dt.Columns.Add("name7", typeof(string));
            dt.Columns.Add("name8", typeof(string));
            dt.Columns.Add("name9", typeof(string));
            for (int i = 0; i < Frame.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["name"] = Frame[i][0];
                dr["name1"] = Frame[i][1];
                dr["name2"] = Frame[i][2];
                dr["name3"] = Frame[i][3];
                dr["name4"] = Frame[i][4];
                dr["name5"] = Frame[i][5];
                dr["name6"] = Frame[i][6];
                dr["name7"] = Frame[i][7];
                dr["name8"] = Frame[i][8];
                dr["name9"] = Frame[i][10];
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private void bdnInfo_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "关闭")
            {
                Clear();
            }
            if (e.ClickedItem.Text == "上一页")
            {
                pageCurrent--;
                if (pageCurrent <= 0)
                {
                    MessageBox.Show("已经是第一页，请点击“下一页”查看！");
                    pageCurrent++;
                    return;
                }
                else
                {
                    nCurrent = pageSize * (pageCurrent - 1);
                }

                LoadData();
            }
            if (e.ClickedItem.Text == "下一页")
            {
                pageCurrent++;
                if (pageCurrent > pageCount)
                {
                    MessageBox.Show("已经是最后一页，请点击“上一页”查看！");
                    pageCurrent--;
                    return;
                }
                else
                {
                    nCurrent = pageSize * (pageCurrent - 1);
                }
                LoadData();
            }
            if (e.ClickedItem.Text == "跳转到")
            {
                int a = 0;
                int.TryParse(txtCurrentPage.Text, out a);
                if (a == 0)
                {
                    MessageBox.Show("只能输入正整数!");
                    return;
                }
                //pageCurrent = int.Parse(txtCurrentPage.Text);
                pageCurrent = a;
                if (pageCurrent <= 0)
                {
                    MessageBox.Show("已经是第一页，请点击“下一页”查看！");
                    return;
                }
                if (pageCurrent > pageCount)
                {
                    MessageBox.Show("已经是最后一页，请点击“上一页”查看！");
                    return;
                }
                nCurrent = pageSize * (pageCurrent - 1);
                LoadData();
            }
        }
    }
}
