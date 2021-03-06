﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnpackPcap
{
    unsafe public sealed class TcpData : PacapData
    {
        public TcpHeader TcpHeader { get; set; }
        protected override int SourcePort
        {
            get { return this.TcpHeader.SrcPort; }
        }
        unsafe public override int HeaderLen
        {
            get { return sizeof(TcpHeader); }
        }
        protected override int DestPort
        {
            get { return this.TcpHeader.DestPort; }
        }
        public override void Fill(byte[] buffer, int offset, int len)
        {
            fixed (byte* ptr = &buffer[offset])
            {
                this.TcpHeader = *(TcpHeader*)ptr;
                this.Data = new byte[len - sizeof(TcpHeader)];
                Array.Copy(buffer, offset + sizeof(TcpHeader), this.Data, 0, this.Data.Length);
            }
        }
    }
}
