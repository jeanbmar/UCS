using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UCS.Core;
using UCS.PacketProcessing;
using UCS.GameFiles;
using UCS.Helpers;

namespace UCS.Logic
{
    class Base
    {
        private int m_vUnknown1;

        public Base(int unknown1)
        {
            m_vUnknown1 = unknown1;
        }

        public virtual void Decode(byte[] baseData)
        {
            using (var br = new BinaryReader(new MemoryStream(baseData)))
            {
                m_vUnknown1 = br.ReadInt32WithEndian();
            }
        }

        public virtual byte[] Encode()
        {
            List<Byte> data = new List<Byte>();
            data.AddInt32(m_vUnknown1);
            return data.ToArray();
        }
    }
}
