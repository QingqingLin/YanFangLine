using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MonitorPorts
{
    class HandlePacket
    {
        PacketProperties PacketProperties;
        static Dictionary<string, DateTime> DicInterval = new Dictionary<string, DateTime>();

        public HandlePacket(byte[] buf, int len)
        {
            PacketProperties = new PacketProperties(len);
            PacketProperties.CaptureTime = DateTime.Now;
            Unpack(buf, len);
        }

        unsafe private void Unpack(byte[] buf, int len)
        {
            byte protocol = 0;
            uint version = 0;
            uint ipSourceAddress = 0;
            uint ipDestinationAddress = 0;
            int sourcePort = 0;
            int destinationPort = 0;
            IPAddress ip;

            Array.Copy(buf, PacketProperties.ReceiveBuf, len);
            PacketProperties.BufLength = len;
            fixed (byte* FixedBuf = buf)
            {
                IPHeader* head = (IPHeader*)FixedBuf;
                PacketProperties.IPHeaderLength = (uint)((head->versionAndLength & 0x0f) << 2);
                protocol = head->protocol;
                version = (uint)((head->versionAndLength & 0xf0) >> 4);
                if (protocol == 17 && version == 4)
                {
                    ipSourceAddress = head->sourceAddress;
                    ipDestinationAddress = head->destinationAdress;
                    ip = new IPAddress(ipSourceAddress);
                    PacketProperties.OriginationAddress = ip.ToString();
                    ip = new IPAddress(ipDestinationAddress);
                    PacketProperties.DestinationAddress = ip.ToString();
                    sourcePort = buf[PacketProperties.IPHeaderLength] * 256 + buf[PacketProperties.IPHeaderLength + 1];
                    destinationPort = buf[PacketProperties.IPHeaderLength + 2] * 256 + buf[PacketProperties.IPHeaderLength + 3];
                    PacketProperties.OriginationPort = sourcePort.ToString();
                    PacketProperties.DestinationPort = destinationPort.ToString();
                    PacketProperties.PacketLength = Convert.ToUInt32(len);
                    if (PacketProperties.PacketLength > PacketProperties.IPHeaderLength + 40)
                    {
                        PacketProperties.MessageLength = PacketProperties.PacketLength - PacketProperties.IPHeaderLength;
                        Array.Copy(buf, (int)PacketProperties.IPHeaderLength, PacketProperties.MessageBuffer, 0, (int)PacketProperties.MessageLength);
                        DataFilterByIPAndPort(PacketProperties);
                    }
                }

                else if (protocol == 41 && version == 4)
                {
                    ipSourceAddress = head->sourceAddress;
                    ipDestinationAddress = head->destinationAdress;
                    ip = new IPAddress(ipSourceAddress);
                    PacketProperties.OriginationAddress = ip.ToString();
                    ip = new IPAddress(ipDestinationAddress);
                    PacketProperties.DestinationAddress = ip.ToString();
                    sourcePort = buf[PacketProperties.IPHeaderLength + 40] * 256 + buf[PacketProperties.IPHeaderLength + 41];
                    destinationPort = buf[PacketProperties.IPHeaderLength + 42] * 256 + buf[PacketProperties.IPHeaderLength + 43];
                    PacketProperties.OriginationPort = sourcePort.ToString();
                    PacketProperties.DestinationPort = destinationPort.ToString();

                    PacketProperties.PacketLength = Convert.ToUInt32(len);

                    if (PacketProperties.PacketLength > PacketProperties.IPHeaderLength + 40)
                    {
                        PacketProperties.MessageLength = PacketProperties.PacketLength - PacketProperties.IPHeaderLength - 40;
                        Array.Copy(buf, (int)PacketProperties.IPHeaderLength + 40, PacketProperties.MessageBuffer, 0, (int)PacketProperties.MessageLength);
                        DataFilterByIPAndPort(PacketProperties);
                    }
                }
            }
        }

        private void DataFilterByIPAndPort(PacketProperties Properties)
        {
            bool IsSourceMeet = false;
            bool isDestMeet = false;
            ConfigurationProperties SourceDevice = null;
            ConfigurationProperties DestDevice = null;
            foreach (var Source in IPList.ConfigProperties)
            {
                if (Source.IsSourceChoose == true && Properties.OriginationAddress == Source.IP && Properties.OriginationPort == Source.Port)
                {
                    IsSourceMeet = true;
                    SourceDevice = Source;
                    break;
                }
            }
            if (IsSourceMeet)
            {
                foreach (var Dest in IPList.ConfigProperties)
                {
                    if (Dest.IsDestChoose == true && Properties.DestinationAddress==Dest.IP && Properties.DestinationPort == Dest.Port)
                    {
                        isDestMeet = true;
                        DestDevice = Dest;
                        break;
                    }
                }
                if (isDestMeet)
                {
                    if (SetSendDir(SourceDevice,DestDevice,Properties))
                    {
                        UpdateForm(Properties);
                        SaveToPcap(Properties);
                    }
                }
            }
        }

        protected virtual void UpdateForm(PacketProperties Properties)
        {
            ShowThread.ShowStorage.Enqueue(Properties);
        }

        private bool SetSendDir(ConfigurationProperties SourceDevice, ConfigurationProperties DestDevice, PacketProperties Properties)
        {
            if (SourceDevice.Type == "VOBC" && DestDevice.Type == "ZC")
            {
                Properties.SendDir = SendDir.VOBCToZC;
                return true;
            }
            else if (SourceDevice.Type == "ZC" && DestDevice.Type == "VOBC")
            {
                Properties.SendDir = SendDir.ZCToVOBC;
                return true;
            }
            else if (SourceDevice.Type == "CI" && DestDevice.Type == "VOBC")
            {
                Properties.SendDir = SendDir.CIToVOBC;
                return true;
            }
            else if (SourceDevice.Type == "VOBC" && DestDevice.Type == "CI")
            {
                Properties.SendDir = SendDir.VOBCToCI;
                return true;
            }
            else if (SourceDevice.Type == "ATS" && DestDevice.Type == "VOBC")
            {
                Properties.SendDir = SendDir.ATSToVOBC;
                return true;
            }
            else if (SourceDevice.Type == "VOBC" && DestDevice.Type == "ATS")
            {
                Properties.SendDir = SendDir.VOBCToATS;
                return true;
            }
            else
            {
                return false;
            }
        }



        private void SaveToPcap(PacketProperties Properties)
        {
            CatchSocket.TemporaryStorage.Enqueue(Properties);
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct IPHeader
        {
            [FieldOffset(0)]
            public byte versionAndLength;
            [FieldOffset(1)]
            public byte typeOfServices;
            [FieldOffset(2)]
            public ushort totalLength;
            [FieldOffset(4)]
            public ushort identifier;
            [FieldOffset(6)]
            public ushort flagsAndOffset;
            [FieldOffset(8)]
            public byte timeToLive;
            [FieldOffset(9)]
            public byte protocol;
            [FieldOffset(10)]
            public ushort checksum;
            [FieldOffset(12)]
            public uint sourceAddress;
            [FieldOffset(16)]
            public uint destinationAdress;
        }
    }
}
