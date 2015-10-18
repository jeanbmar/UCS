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
    //Commande 0x21F
    class KickAllianceMemberCommand : Command
    {
        private long m_vAvatarId;
        private string m_vMessage;

        public KickAllianceMemberCommand(BinaryReader br)
        {
            m_vAvatarId = br.ReadInt64WithEndian();
            br.ReadByte();
            m_vMessage = br.ReadScString();
            br.ReadInt32WithEndian();
        }

        //00 00 02 24 20 41 6A 6B 00 00 00 01 00 00 02 1F 
        //00 00 00 17 00 9E 81 01 
        //01 
        //00 00 00 33
        //44 C3 A9 73 6F 6C C3 A9 2C 20 6E 6F 75 73 20 61 76 6F 6E 73 20 64 C3 A9 63 69 64 C3 A9 20 64 65 20 74 27 65 78 63 6C 75 72 65 20 64 75 20 63 6C 61 6E 2E 
        //00 00 01 E6

        public override void Execute(Level level)
        {
            Level targetAccount = ResourcesManager.GetPlayer(m_vAvatarId, true);
            if(targetAccount != null)
            {
                var targetAvatar = targetAccount.GetPlayerAvatar();
                var targetAllianceId = targetAvatar.GetAllianceId();
                var requesterAvatar = level.GetPlayerAvatar();
                var requesterAllianceId = requesterAvatar.GetAllianceId();
                if (requesterAllianceId > 0 && targetAllianceId == requesterAllianceId)
                {
                    Alliance alliance = ObjectManager.GetAlliance(requesterAllianceId);
                    var requesterMember = alliance.GetAllianceMember(requesterAvatar.GetId());
                    var targetMember = alliance.GetAllianceMember(m_vAvatarId);
                    if(targetMember.HasLowerRoleThan(requesterMember.GetRole()))
                    {
                        targetAvatar.SetAllianceId(0);
                        alliance.RemoveMember(m_vAvatarId);
                        //Now sending messages
                        if (ResourcesManager.IsPlayerOnline(targetAccount))
                        {
                            var leaveAllianceCommand = new LeaveAllianceCommand();
                            leaveAllianceCommand.SetAlliance(alliance);
                            leaveAllianceCommand.SetReason(2);//kick
                            var availableServerCommandMessage = new AvailableServerCommandMessage(targetAccount.GetClient());
                            availableServerCommandMessage.SetCommandId(2);
                            availableServerCommandMessage.SetCommand(leaveAllianceCommand);
                            PacketManager.ProcessOutgoingPacket(availableServerCommandMessage);

                            AllianceKickOutStreamEntry kickOutStreamEntry = new AllianceKickOutStreamEntry();
                            kickOutStreamEntry.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                            kickOutStreamEntry.SetAvatar(requesterAvatar);
                            kickOutStreamEntry.SetIsNew(0);
                            kickOutStreamEntry.SetAllianceId(alliance.GetAllianceId());
                            kickOutStreamEntry.SetAllianceBadgeData(alliance.GetAllianceBadgeData());
                            kickOutStreamEntry.SetAllianceName(alliance.GetAllianceName());
                            kickOutStreamEntry.SetMessage(m_vMessage);
                            var p = new AvatarStreamEntryMessage(targetAccount.GetClient());
                            p.SetAvatarStreamEntry(kickOutStreamEntry);
                            PacketManager.ProcessOutgoingPacket(p);
                        }

                        AllianceEventStreamEntry eventStreamEntry = new AllianceEventStreamEntry();
                        eventStreamEntry.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                        eventStreamEntry.SetAvatar(targetAvatar);
                        eventStreamEntry.SetEventType(1);
                        eventStreamEntry.SetAvatarId(requesterAvatar.GetId());
                        eventStreamEntry.SetAvatarName(requesterAvatar.GetAvatarName());

                        alliance.AddChatMessage(eventStreamEntry);

                        foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
                        {
                            if (onlinePlayer.GetPlayerAvatar().GetAllianceId() == requesterAllianceId)
                            {
                                var p = new AllianceStreamEntryMessage(onlinePlayer.GetClient());
                                p.SetStreamEntry(eventStreamEntry);
                                PacketManager.ProcessOutgoingPacket(p);
                            }
                        }
                    }
                }
            }
        }
    }
}
