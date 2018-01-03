using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MonitorPorts
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            HardwareInfo hardwareInfo = new HardwareInfo();
            string cpuID = hardwareInfo.GetCpuID();
            string HardID = hardwareInfo.GetHardDiskID();
            if (cpuID == "BFEBFBFF000806E9" && HardID == "0025_38B8_71B6_1961.")  //填主机cpu序列号
            {
                Application.Run(new MainForm());
            }
            else
            {
                MessageBox.Show("非本机，不能使用！");
                Application.Exit();
            }
}
    }
}
