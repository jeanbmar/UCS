using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using UCS.Core;
using UCS.Helpers;
using UCS.Network;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    //14302
    class AskForAllianceDataMessage : Message
    {
        private long m_vAllianceId;

        public AskForAllianceDataMessage(Client client, BinaryReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                m_vAllianceId = br.ReadInt64WithEndian();
            }
        }

        public override void Process(Level level)
        {
            Alliance alliance = ObjectManager.GetAlliance(m_vAllianceId);
            if (alliance != null)
            {
                PacketManager.ProcessOutgoingPacket(new AllianceDataMessage(this.Client, alliance));
            }
        }
    }
}
