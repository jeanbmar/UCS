using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Logic;
using UCS.Core;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Packet 24115
    class ServerErrorMessage : Message
    {
        private string m_vErrorMessage;

        public ServerErrorMessage(Client client)
            : base(client)
        {
            SetMessageType(24115);
        }

        public override void Encode()
        {
            List<Byte> data = new List<Byte>();
            data.AddString(m_vErrorMessage);
            SetData(data.ToArray());
        }

        public void SetErrorMessage(string message)
        {
            m_vErrorMessage = message;
        }
    }
}
