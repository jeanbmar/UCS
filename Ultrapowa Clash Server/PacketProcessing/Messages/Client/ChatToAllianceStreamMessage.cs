using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using UCS.Helpers;
using UCS.Logic;
using UCS.Network;
using UCS.Core;

namespace UCS.PacketProcessing
{
    //14315
    class ChatToAllianceStreamMessage : Message
    {
        private string m_vChatMessage;

        public ChatToAllianceStreamMessage(Client client, BinaryReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                m_vChatMessage = br.ReadScString();
            }   
        }

        public override void Process(Level level)
        {
            var avatar = level.GetPlayerAvatar();
            var allianceId = avatar.GetAllianceId();
            if (allianceId > 0)
            {
                ChatStreamEntry cm = new ChatStreamEntry();
                cm.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                cm.SetAvatar(avatar);
                cm.SetMessage(m_vChatMessage);

                Alliance alliance = ObjectManager.GetAlliance(allianceId);
                if (alliance != null)
                {
                    alliance.AddChatMessage(cm);

                    foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
                    {
                        if(onlinePlayer.GetPlayerAvatar().GetAllianceId() == allianceId)
                        {
                            var p = new AllianceStreamEntryMessage(onlinePlayer.GetClient());
                            p.SetStreamEntry(cm);
                            PacketManager.ProcessOutgoingPacket(p);
                        }
                    }
                }
            }
        }
    }
}
