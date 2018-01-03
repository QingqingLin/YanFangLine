using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonitorPorts
{
    public class PacketProperties
    {
        private byte[] _ReceiveBuf;
        private int _BufLength;
        private string destinationPort;
        private string originationPort;
        private string destinationAddress;
        private string originationAddress;
        private string version;
        private uint packetLength;
        private uint messageLength;
        private uint ipHeaderLength;
        private byte[] messageBuffer = null;
        private DateTime _captureTime = DateTime.Now;
        private double interval;
        private SendDir _sendDir;


        public PacketProperties(int receiveBufferLength)
        {
            _ReceiveBuf = new byte[receiveBufferLength];
            messageBuffer = new byte[receiveBufferLength];
        }

        public SendDir SendDir
        {
            get { return _sendDir; }
            set { _sendDir = value; }
        }
        
        public byte[] ReceiveBuf
        {
            get { return _ReceiveBuf; }
            set { _ReceiveBuf = value; }
        }

        public double Interval
        {
            get { return interval; }
            set { interval = value; }
        }

        public int BufLength
        {
            get { return _BufLength; }
            set { _BufLength = value; }
        }

        public string DestinationPort
        {
            get { return destinationPort; }
            set { destinationPort = value; }
        }

        public string OriginationPort
        {
            get { return originationPort; }
            set { originationPort = value; }
        }

        public string DestinationAddress
        {
            get { return destinationAddress; }
            set { destinationAddress = value; }
        }

        public string OriginationAddress
        {
            get { return originationAddress; }
            set { originationAddress = value; }
        }

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public uint PacketLength
        {
            get { return packetLength; }
            set { packetLength = value; }
        }

        public uint MessageLength
        {
            get { return messageLength; }
            set { messageLength = value; }
        }


        public uint IPHeaderLength
        {
            get { return ipHeaderLength; }
            set { ipHeaderLength = value; }
        }

        public byte[] MessageBuffer
        {
            get { return messageBuffer; }
            set { messageBuffer = value; }
        }

        public DateTime CaptureTime
        {
            get { return _captureTime; }
            set { _captureTime = value; }
        }
    }
}
