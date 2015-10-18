using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    //Packet 24111
    class LeaveAllianceOkMessage : Message
    {
        private int m_vServerCommandType;
        private Alliance m_vAlliance;

        public LeaveAllianceOkMessage(Client client, Alliance alliance)
            : base(client)
        {
            SetMessageType(24111);

            m_vServerCommandType = 0x02;
            m_vAlliance = alliance;
        }

        //00 00 00 02 00 00 00 3B 00 0A 40 1E 00 00 00 01 FF FF FF FF
        public override void Encode()
        {
            List<Byte> pack = new List<Byte>();

            pack.AddInt32(m_vServerCommandType);
            pack.AddInt64(m_vAlliance.GetAllianceId());
            pack.AddInt32(1);//reason? 1= leave, 2=kick
            pack.AddInt32(-1);

            SetData(pack.ToArray());
        }
    }
}
