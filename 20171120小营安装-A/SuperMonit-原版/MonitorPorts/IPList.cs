using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MonitorPorts
{
    class IPList
    {
        public static List<ConfigurationProperties> ConfigProperties = new List<ConfigurationProperties>();

        public IPList()
        {
            string path = "IPlist.csv";
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                ConfigurationProperties Configuration = new ConfigurationProperties();
                string[] strs = line.Split(new char[] { ',' });
                Configuration.Name = strs[0];
                Configuration.IP = strs[1];
                Configuration.Port = strs[2];
                Configuration.Type = strs[3];
                ConfigProperties.Add(Configuration);
            }
        }
    }
    class ConfigurationProperties
    {
        public string Name { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public string Type { get; set; }
        public bool IsSourceChoose { get; set; }
        public bool IsDestChoose { get; set; }
    }
}
