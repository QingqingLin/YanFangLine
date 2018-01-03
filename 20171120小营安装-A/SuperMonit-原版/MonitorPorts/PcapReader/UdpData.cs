using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace UnpackPcap
{
    public sealed class UdpData : PacapData
    {
        public UDPHeader UdpHeader = new UDPHeader();
        unsafe public override int HeaderLen
        {
            get { return sizeof(UDPHeader); }
        }
        protected override int SourcePort

        {
            get { return (int)this.UdpHeader.SrcPort; }
        }

        protected override int DestPort
        {
            get { return (int)this.UdpHeader.DestPort; }
        }

        unsafe public override void Fill(byte[] buffer, int offset, int len)
        {
            this.UdpHeader.SrcPort = (ushort)(buffer[offset + 12] * 256 + buffer[offset + 13]);
            this.UdpHeader.DestPort = (ushort)(buffer[offset + 14] * 256 + buffer[offset + 15]);
            this.UdpHeader.Length = (ushort)(buffer[offset + 16] * 256 + buffer[offset + 17]);
            this.UdpHeader.CheckSum = (ushort)(buffer[offset + 18] * 256 + buffer[offset + 19]);

            this.Data = new byte[len - sizeof(UDPHeader) - 12];
            Array.Copy(buffer, offset + 12 + sizeof(UDPHeader), this.Data, 0, this.Data.Length);
    }
    }
}