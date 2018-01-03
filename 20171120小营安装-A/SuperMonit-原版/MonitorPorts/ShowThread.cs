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
        public static BlockingCollection<PacketProperties> ShowStorage;
        public delegate void PacketArrivedEventHandler(PacketProperties Properties);
        public static event PacketArrivedEventHandler PacketArrival;

        public static BlockingCollection<PacketProperties> GUIStorage1 = new BlockingCollection<PacketProperties>();
        public static BlockingCollection<PacketProperties> GUIStorage2 = new BlockingCollection<PacketProperties>();


        public static void Show(object o)
        {
            if (ShowStorage == GUIStorage1)
            {
                GUIStorage2 = new BlockingCollection<PacketProperties>();
                ShowStorage = GUIStorage2;
                OutToForm(GUIStorage1);
            }
            else if (ShowStorage == GUIStorage2)
            {
                GUIStorage1 = new BlockingCollection<PacketProperties>();
                ShowStorage = GUIStorage1;
                OutToForm(GUIStorage2);
            }
        }

        private static void OutToForm(BlockingCollection<PacketProperties> guiStorage)
        {
            foreach (var Properties in guiStorage)
            {
                if (PacketArrival != null)
                {
                    PacketArrival(Properties);
                }
            }
            guiStorage.Dispose();
        }
    }
}