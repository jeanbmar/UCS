using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    class Message
    {
        private ushort m_vType;
        private int m_vLength;
        private byte[] m_vData;
        private ushort m_vMessageVersion;
        public Client Client
        {
            get;
            set;
        }
        public int Broadcasting
        {
            get;
            set;
        }

        public Message() { }

        public Message(Client c) 
        {
            this.Client = c;
            m_vType = 0;
            m_vLength = -1;
            m_vMessageVersion = 0;
            m_vData = null;
        }

        public Message(Client c, Message m) //Clone
        {
            m_vType = m.GetMessageType();
            m_vLength = m.GetLength();
            m_vData = new byte[m.GetLength()];
            Array.Copy(m.GetData(), m_vData, m.GetLength());
            m_vMessageVersion = m.GetMessageVersion();
            this.Client = c;
        }

        public Message(Client c, BinaryReader br)
        {
            this.Client = c;
            m_vType = br.ReadUInt16WithEndian();
            byte[] tempLength = br.ReadBytes(3);
            m_vLength = ((0x00 << 24) | (tempLength[0] << 16) | (tempLength[1] << 8) | tempLength[2]);
            m_vMessageVersion = br.ReadUInt16WithEndian();
            m_vData = br.ReadBytes(m_vLength);
        }

        public override String ToString()
        {
            return Encoding.UTF8.GetString(this.m_vData, 0, m_vLength);
        }

        public String ToHexString()
        {
            String hex = BitConverter.ToString(this.m_vData);
            return hex.Replace("-"," ");
        }

        public byte[] GetRawData()
        {
            List<Byte> encodedMessage = new List<Byte>();

            encodedMessage.AddRange(BitConverter.GetBytes(this.m_vType).Reverse());
            encodedMessage.AddRange(BitConverter.GetBytes(this.m_vLength).Reverse().Skip(1));
            encodedMessage.AddRange(BitConverter.GetBytes(this.m_vMessageVersion).Reverse());
            encodedMessage.AddRange(m_vData);

            return encodedMessage.ToArray();
        }

        public virtual void Encode()
        {
            
        }

        public virtual void Decode()
        {

        }

        public virtual void Process(Level level)
        {

        }

        public ushort GetMessageType()
        {
            return m_vType;
        }

        public void SetMessageType(ushort type)
        {
            m_vType = type;
        }

        public byte[] GetData()
        {
            return m_vData;
        }

        public void SetData(byte[] data)
        {
            m_vData = data;
            m_vLength = data.Length;
        }

        public int GetLength()
        {
            return m_vLength;
        }

        public ushort GetMessageVersion()
        {
            return m_vMessageVersion;
        }

        public void SetMessageVersion(ushort v)
        {
            m_vMessageVersion = v;
        }
    }
}
