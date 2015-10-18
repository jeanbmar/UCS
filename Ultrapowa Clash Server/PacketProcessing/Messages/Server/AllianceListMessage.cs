using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Logic;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Packet 24310
    class AllianceListMessage : Message
     {
        private List<Alliance> m_vAlliances;
        private string m_vSearchString;

        public AllianceListMessage(Client client) : base(client)
        {
            SetMessageType(24310);
            m_vAlliances = new List<Alliance>();
        }

        //00 00 00 0B 
        //73 65 61 72 63 68 20 74 65 78 74 
        //00 00 00 01 
        //00 00 00 1E 00 1E A4 D8 00 00 00 0F 53 65 61 72 63 68 53 74 61 72 43 6C 61 6E 73 61 00 13 52 00 00 00 02 00 00 00 08 00 00 10 89 00 00 00 00 00 00 00 0C 00 00 00 10 00 00 00 01 00 1E 84 80 00 00 00 02 01 E8 48 E7 00 00 00 AA 00 00 00 04
        public override void Encode()
        {
            List<Byte> pack = new List<Byte>();

            pack.AddString(m_vSearchString);
            pack.AddInt32(m_vAlliances.Count);
            foreach(var alliance in m_vAlliances)
            {
                pack.AddRange(alliance.EncodeFullEntry());
            }

            SetData(pack.ToArray());
        }

        public void SetAlliances(List<Alliance> alliances)
        {
            m_vAlliances = alliances;
        }
 
        public void SetSearchString(string searchString)
        {
            m_vSearchString = searchString;
        }
    }
}
