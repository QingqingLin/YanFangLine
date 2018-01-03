using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace MonitorPorts
{
    public class Communication
    {

        private Socket socketMonitor;

        //定义接收和发送端口号
        private const int remotePort = 12580;
        private const int localPort = 12570;

        private byte[] sendBuffer = new byte[128];
        private byte[] receiveBuffer = new byte[512];
        string strSend;

        private IPEndPoint localEndPoint;
        private IPEndPoint remoteEndPoint;
        private EndPoint senderRemote;

        private bool _isRunning;    //定义标志位，监控被监测程序是否正在运行状态
        private DateTime _receiveTime;

        #region//属性
        public bool IsRunning
        {
            get { return _isRunning; }
            set { _isRunning = value; }
        }

        public Socket SocketMonitor
        {
            get { return socketMonitor; }
            set { socketMonitor = value; }
        }

        public DateTime ReceiveTime
        {
            get { return _receiveTime; }
            set { _receiveTime = value; }
        }

        public string StrSend
        {
            get { return strSend; }
            set { strSend = value; }
        }
        #endregion
       
        /// <summary>
        /// 构造函数
        /// </summary>
        public Communication()
        {
            this.StrSend = "1" + "轨道交通控制与安全国家重点实验室";   //发送当前系统时间，首字符1作为判断标志位
            ReceiveTime = DateTime.Now;
            IsRunning = false;
            this.BindPort();

        }
        /// <summary>
        /// 绑定端口号
        /// </summary>
        public void BindPort()
        {
            IPHostEntry localIpEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress localIpAddr = null;
            foreach (IPAddress ip in localIpEntry.AddressList)              //得到本地IP地址
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIpAddr = ip;
                    break;
                }
            }
            localEndPoint = new IPEndPoint(localIpAddr, localPort);     //接收端口号
            remoteEndPoint = new IPEndPoint(localIpAddr, remotePort);   //接收端口号
            senderRemote = (EndPoint)(new IPEndPoint(IPAddress.Any, 0));         
            socketMonitor = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);           
            try
            {
                socketMonitor.Bind(localEndPoint);

            }
            catch (Exception ex)
            {
                MessageBox.Show("开始通信失败!" + ex.Message);
            }
        }

        /// <summary>
        /// 发送函数
        /// </summary>
        public void Send()
        {
            while (true)
            {
                Thread.Sleep(2000);             //每隔2秒发送一次
                char[] bufferC = strSend.ToCharArray();
                byte[] buffer = Encoding.Default.GetBytes(bufferC);
                Array.Copy(buffer, sendBuffer, buffer.Length);      //将发送的字符串转为byte数组，并拷贝到sendBuffer中，为了确保每次发送长度为128位
                try
                {
                    socketMonitor.SendTo(sendBuffer, remoteEndPoint);       //发送数据 sendBuffer
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Monitor端发送异常" + ex.Message);
                }
            }

        }
        /// <summary>
        /// 发送5次，保证不会误判
        /// </summary>
        public void SendFiveTimes()
        {
            string StrSendFive = "0" + "轨道交通控制与安全国家重点实验室";
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(50);
                char[] bufferC = StrSendFive.ToCharArray();
                byte[] buffer = Encoding.Default.GetBytes(bufferC);
                Array.Copy(buffer, sendBuffer, buffer.Length);
                try
                {
                    socketMonitor.SendTo(sendBuffer, remoteEndPoint);
                }
                catch (ArgumentNullException e)
                {
                    MessageBox.Show(e.Message);
                }
                catch (SocketException e)
                {
                    MessageBox.Show(e.Message);
                }

            }
        }

        /// <summary>
        /// 接收函数
        /// </summary>
        public void ReceiveMessages()
        {     
            while (true)
            {
                try
                {
                    int receiveNum = socketMonitor.ReceiveFrom(receiveBuffer, ref senderRemote);
                    if (receiveNum != 0)
                    {
                        ReceiveTime = DateTime.Now;
                        IsRunning = true;                      
                    }
                }
                catch
                {
                    Thread.Sleep(1000); //如果没有收到消息，就休眠一秒，之后再次接受                   
                }
                Array.Clear(receiveBuffer, 0, receiveBuffer.Length);
            }
        
        }


     

        /// <summary>
        /// 判断函数
        /// </summary>
        public void Check()
        {
            while (true)
            {
                Thread.Sleep(5000);     //每隔四秒判定一次
                if (IsRunning)
                {
                    if (this.ReceiveTime.AddSeconds(16) < DateTime.Now)
                    {
                        Restart();
                        IsRunning = false;
                    }
                }
            }
        }

        /// <summary>
        /// 重启函数
        /// </summary>
        public void Restart()
        {
            Process[] myProcess = Process.GetProcesses();
            for (int i = 0; i < myProcess.Length; i++)
            {
                if (myProcess[i].ProcessName == "MonitorWatcher")
                {
                    myProcess[i].Kill();
                    myProcess[i].Close();
                    myProcess[i].Dispose();
                }
            }
            Process myNewProcess = new Process();
            myNewProcess.StartInfo.FileName = "MonitorWatcher.exe";
            myNewProcess.StartInfo.WorkingDirectory = Application.StartupPath;
            myNewProcess.Start();
        }


    }
}

