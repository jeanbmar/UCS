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
    //Packet 24104
    class OutOfSyncMessage : Message
    {
        public OutOfSyncMessage(Client client) : base(client) 
        {
            SetMessageType(24104);
        }

        public override void Encode()
        {
            List<Byte> data = new List<Byte>();
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);
            SetData(data.ToArray());
        }
    }
}
