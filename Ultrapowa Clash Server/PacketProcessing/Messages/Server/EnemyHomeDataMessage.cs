using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using Newtonsoft.Json;
using UCS.Logic;
using Ionic.Zlib;

namespace UCS.PacketProcessing
{
    //Packet 24107
    class EnemyHomeDataMessage : Message
    {
        private Level m_vOwnerLevel;
        private Level m_vVisitorLevel;

        public EnemyHomeDataMessage(Client client, Level ownerLevel, Level visitorLevel)
            : base(client)
        {
            SetMessageType(24107);

            m_vOwnerLevel = ownerLevel;
            m_vVisitorLevel = visitorLevel;
        }

        public override void Encode()
        {
            List<Byte> data = new List<Byte>();

            //data.AddRange(BitConverter.GetBytes(Player.GetPlayerAvatar().GetSecondsFromLastUpdate()).Reverse());
            data.AddRange(new byte[]{
                0x00, 0x00, 0x00, 0xF0, 
                0xFF, 0xFF, 0xFF, 0xFF, 
                0x54, 0xCE, 0x5C, 0x4A
            });

            ClientHome ch = new ClientHome(m_vOwnerLevel.GetPlayerAvatar().GetId());
            ch.SetShieldDurationSeconds(m_vOwnerLevel.GetPlayerAvatar().RemainingShieldTime);
            ch.SetHomeJSON(m_vOwnerLevel.SaveToJSON());

            data.AddRange(ch.Encode());
            data.AddRange(m_vOwnerLevel.GetPlayerAvatar().Encode());

            data.AddRange(m_vVisitorLevel.GetPlayerAvatar().Encode());

            data.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x03, 0x00 });

            SetData(data.ToArray());
        }
    }
}
