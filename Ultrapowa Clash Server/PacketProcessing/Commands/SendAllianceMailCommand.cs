using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using UCS.Logic;
using UCS.Helpers;
using UCS.GameFiles;
using UCS.Core;
using UCS.Network;

namespace UCS.PacketProcessing
{
    //Commande 0x219
    class SendAllianceMailCommand : Command
    {
        private string m_vMailContent;

        public SendAllianceMailCommand(BinaryReader br)
        {
            m_vMailContent = br.ReadScString();
            br.ReadInt32WithEndian();
        }

        public override void Execute(Level level)
        {
            var avatar = level.GetPlayerAvatar();
            var allianceId = avatar.GetAllianceId();
            if (allianceId > 0)
            {
                var alliance = ObjectManager.GetAlliance(allianceId);
                if (alliance != null)
                {
                    AllianceMailStreamEntry mail = new AllianceMailStreamEntry();
                    mail.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                    mail.SetAvatar(avatar);
                    mail.SetIsNew(0);
                    mail.SetSenderId(avatar.GetId());
                    mail.SetAllianceId(allianceId);
                    mail.SetAllianceBadgeData(alliance.GetAllianceBadgeData());
                    mail.SetAllianceName(alliance.GetAllianceName());
                    mail.SetMessage(m_vMailContent);

                    foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
                    {
                        if (onlinePlayer.GetPlayerAvatar().GetAllianceId() == allianceId)
                        {
                            var p = new AvatarStreamEntryMessage(onlinePlayer.GetClient());
                            p.SetAvatarStreamEntry(mail);
                            PacketManager.ProcessOutgoingPacket(p);
                        }
                    }
                }
            }
        }
    }
}
