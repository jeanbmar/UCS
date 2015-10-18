using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    //Packet 24312
    class AllianceStreamEntryMessage : Message
    {
        private StreamEntry m_vStreamEntry;

        public AllianceStreamEntryMessage(Client client) : base(client)
        {
            SetMessageType(24312);
        }

        public void SetStreamEntry(StreamEntry entry)
        {
            m_vStreamEntry = entry;
        }

        public override void Encode()
        {
            List<Byte> pack = new List<Byte>();

            pack.AddRange(m_vStreamEntry.Encode());

            SetData(pack.ToArray());
        }
    }
}
