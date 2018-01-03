using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonitorPorts
{
    class ShowThread
    {
        public static ConcurrentQueue<PacketProperties> ShowStorage;
        public delegate void PacketArrivedEventHandler(PacketProperties Properties);
        public static event PacketArrivedEventHandler PacketArrival;


        public static void Show(object o)
        {
            OutToForm(ShowStorage);
        }

        private static void OutToForm(ConcurrentQueue<PacketProperties> guiStorage)
        {
            while (!guiStorage.IsEmpty)
            {
                PacketProperties Properties;
                guiStorage.TryDequeue(out Properties);
                if (PacketArrival != null)
                {
                    PacketArrival(Properties);
                }
            }
        }
    }
}