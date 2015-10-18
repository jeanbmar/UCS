using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using Newtonsoft.Json;
using UCS.Logic;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Packet 24113
    class VisitedHomeDataMessage : Message
    {
        private Level m_vOwnerLevel;
        private Level m_vVisitorLevel;

        public VisitedHomeDataMessage(Client client, Level ownerLevel, Level visitorLevel) : base (client)
        {
            SetMessageType(24113);
            m_vOwnerLevel = ownerLevel;
            m_vVisitorLevel = visitorLevel;
        }

        public override void Encode()
        {
            List<Byte> data = new List<Byte>();

            //data.AddRange(BitConverter.GetBytes(Player.GetPlayerAvatar().GetSecondsFromLastUpdate()).Reverse());
            data.AddInt32(0);//replace previous after patch

            ClientHome ch = new ClientHome(m_vOwnerLevel.GetPlayerAvatar().GetId());
            ch.SetShieldDurationSeconds(m_vOwnerLevel.GetPlayerAvatar().RemainingShieldTime);
            ch.SetHomeJSON(m_vOwnerLevel.SaveToJSON());

            data.AddRange(ch.Encode());
            data.AddRange(m_vOwnerLevel.GetPlayerAvatar().Encode());

            data.Add(1);
            data.AddRange(m_vVisitorLevel.GetPlayerAvatar().Encode());

            SetData(data.ToArray());
        }
    }
}
