using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Logic;
using UCS.Core;

namespace UCS.PacketProcessing
{
    //Packet 20108
    class KeepAliveOkMessage : Message
    {
        public KeepAliveOkMessage(Client client, KeepAliveMessage cka)
            : base(client)
        {
            SetMessageType(20108);
        }

        public override void Encode()
        {
            List<Byte> data = new List<Byte>();
            SetData(data.ToArray());
        }
    }
}
