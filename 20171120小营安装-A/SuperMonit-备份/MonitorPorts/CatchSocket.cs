using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MonitorPorts
{
    class CatchSocket
    {
        public bool KeepCatching;
        public GeneratePcapFile genePcapFile;
        private static int receiveBufferLength;
        private byte[] receiveBufferBytes;
        private Socket socket;
        private string _fileSavePath;
        private string _fileName;
        private delegate void UpdateState();        
        HandlePacket Handle;
        public static List<PacketProperties> TemporaryStorage;

        public string FileSavePath
        {
            get { return _fileSavePath; }
            set { _fileSavePath = value; }
        }

        public CatchSocket(MainForm Main)
        {
            receiveBufferLength = 2000;
            receiveBufferBytes = new byte[receiveBufferLength];
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public void BeginReceive()
        {
            if (socket != null)
            {
                object state = null;
                state = socket;
                IAsyncResult ar = socket.BeginReceive(receiveBufferBytes, 0, receiveBufferLength, SocketFlags.None, new AsyncCallback(CallReceive), state);
            }
        }

        private void CallReceive(IAsyncResult ar)
        {
            try
            {
                int receivedBytes = socket.EndReceive(ar);
                if (KeepCatching == true)
                {
                    Handle = new HandlePacket(receiveBufferBytes, receivedBytes);
                    Array.Clear(receiveBufferBytes, 0, receiveBufferBytes.Length);
                }
            }
            catch (Exception)
            {

            }
            BeginReceive();
        }

        public void Start(Socket socket)
        {
            if (MainForm.IsFirstClick)
            {
                MainForm.OutputDataTimer.Change(1000, 5000);
                MainForm.ShowTimer.Change(200, 200);
                this.socket = socket;
                KeepCatching = true;
                SetTemporaryStorge();
                SetShowStorge();
                BeginReceive();
            }
            else
            {
                MainForm.OutputDataTimer.Change(1000, 5000);
                MainForm.ShowTimer.Change(200, 200);
                KeepCatching = true;
            }
        }

        private void SetTemporaryStorge()
        {
            if (TemporaryStorage == null)
            {
                TemporaryStorage = MainForm.TemporaryStorage1;
            }
        }

        private void SetShowStorge()
        {
            if (ShowThread.ShowStorage == null)
            {
               ShowThread.ShowStorage = ShowThread.GUIStorage1;
            }
        }

        public void Stop()
        {
            KeepCatching = false;
        }

        public void CreatPcapFile()
        {
            genePcapFile = new GeneratePcapFile();
            FileName = System.DateTime.Now.ToString("yyyy") + "." + System.DateTime.Now.ToString("MM") + "." + System.DateTime.Now.ToString("dd") + "  " + System.DateTime.Now.ToString("HH：mm");
            FileSavePath = SetFileSavePath();
            if (File.Exists(FileSavePath + "\\" + FileName + ".pcap"))
            {
                File.Delete(FileSavePath + "\\" + FileName + ".pcap");
                genePcapFile.CreatPcap(FileSavePath, FileName);
            }
            else
            {
                genePcapFile.CreatPcap(FileSavePath, FileName);
            }
        }

        public void CreatPcapFilePerDay(object o)
        {
            genePcapFile = new GeneratePcapFile();
            FileName = System.DateTime.Now.ToString("yyyy") + "." + System.DateTime.Now.ToString("MM") + "." + System.DateTime.Now.ToString("dd") + "  " + System.DateTime.Now.ToString("HH：mm");
            FileSavePath = SetFileSavePath();
            genePcapFile.CreatPcap(FileSavePath, FileName);

            DateTime dt1 = DateTime.Parse(DateTime.Now.ToShortDateString() + " 23:59:59");
            DateTime dt2 = DateTime.Now;
            TimeSpan ts = new TimeSpan();
            ts = dt1 - dt2;
            Int64 IntervalToTomorrow0 = Convert.ToInt64(ts.TotalMilliseconds + 1800000);
            MainForm.CreatPcapPerDay.Change(IntervalToTomorrow0, IntervalToTomorrow0);
        }

        private string SetFileSavePath()
        {
            string startPath = Application.StartupPath + "\\pcap";
            string FilePath = startPath + "\\" + System.DateTime.Now.Year + "\\" + System.DateTime.Now.Month + "\\" + System.DateTime.Now.Day;
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
            return FilePath;
        }

        public void OutToPcap(object o)
        {
            if (TemporaryStorage == MainForm.TemporaryStorage1)
            {
                TemporaryStorage = MainForm.TemporaryStorage2;
                Output(MainForm.TemporaryStorage1);
            }
            else if (TemporaryStorage == MainForm.TemporaryStorage2)
            {
                TemporaryStorage = MainForm.TemporaryStorage1;
                Output(MainForm.TemporaryStorage2);
            }
        }

        private void Output(List<PacketProperties> PropertiesList)
        {
            FileInfo File = new FileInfo(FileSavePath + "\\" + FileName + ".pcap");
            lock (PropertiesList)
            {
                foreach (PacketProperties Properties in PropertiesList)
                {
                    if (File.Length < FilterForm.PcapLengthNum)
                    {
                        WriteToPcap(Properties.ReceiveBuf, Properties.BufLength, Properties.CaptureTime);
                    }
                    else
                    {
                        CreatPcapFile();
                        File = new FileInfo(FileSavePath + "\\" + FileName + ".pcap");
                        WriteToPcap(Properties.ReceiveBuf, Properties.BufLength, Properties.CaptureTime);
                    }
                }
                PropertiesList.Clear();
            }
        }

        private void WriteToPcap(byte[] packet, int packetlenth, DateTime CaptureTime)
        {
            genePcapFile.WritePacketData(CaptureTime, packet, packetlenth);
        }
    }
}
