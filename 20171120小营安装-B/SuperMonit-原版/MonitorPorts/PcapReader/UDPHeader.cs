using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnpackPcap
{
    public struct UDPHeader
    {
        /// <summary>
        /// 源端口
        /// </summary>
        public UInt16 SrcPort;
        /// <summary>
        /// 目的端口
        /// </summary>
        public UInt16 DestPort;
        /// <summary>
        /// udp包长度
        /// </summary>
        public UInt16 Length;
        /// <summary>
        /// 校验和
        /// </summary>
        public UInt16 CheckSum;
    }
}
