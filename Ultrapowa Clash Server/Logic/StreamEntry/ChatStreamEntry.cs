using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.PacketProcessing;
using UCS.Helpers;

namespace UCS.Logic
{
    class ChatStreamEntry : StreamEntry
    {
        private string m_vMessage;
        
        public ChatStreamEntry() : base()
        {
        }

        public string GetMessage()
        {
            return m_vMessage;
        }

        public override int GetStreamEntryType()
        {
            return 2;
        }

        public override byte[] Encode()
        {
            List<Byte> data = new List<Byte>();

            data.AddRange(base.Encode());
            data.AddString(m_vMessage);

            return data.ToArray();
        }

        public void SetMessage(string message)
        {
            m_vMessage = message;
        }
    }    
}
