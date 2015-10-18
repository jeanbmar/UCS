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
    //Packet 14301
    class CreateAllianceMessage : Message
    {
        private string m_vAllianceName;
        private string m_vAllianceDescription;
        private int m_vAllianceBadgeData;
        private int m_vAllianceType;
        private int m_vRequiredScore;
        private int m_vWarFrequency;
        private int m_vAllianceOrigin;

        public CreateAllianceMessage(Client client, BinaryReader br) : base (client, br)
        {
        }

        //00 00 00 04 6E 61 6D 65 00 00 00 0B 64 65 73 63 72 69 70 74 69 6F 6E 5B 00 02 52 00 00 00 01 00 00 07 D0 00 00 00 02 01 E8 48 39
        //00 00 00 04 6E 61 6D 65 00 00 00 0B 64 65 73 63 72 69 70 74 69 6F 6E 00 00 00 00 00 00 00 02 00 00 07 D0 00 00 00 02 01 E8 48 3A
        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                m_vAllianceName = br.ReadScString(); //6E 61 6D 65
                m_vAllianceDescription = br.ReadScString();//64 65 73 63 72 69 70 74 69 6F 6E
                m_vAllianceBadgeData = br.ReadInt32WithEndian();//5B 00 02 52
                m_vAllianceType = br.ReadInt32WithEndian();//00 00 00 01
                m_vRequiredScore = br.ReadInt32WithEndian();//00 00 07 D0
                m_vWarFrequency = br.ReadInt32WithEndian();//00 00 00 02
                m_vAllianceOrigin = br.ReadInt32WithEndian();//01 E8 48 39
            }
        }

        public override void Process(Level level)
        {
            //Clan creation
            Alliance alliance = ObjectManager.CreateAlliance(0);
            alliance.SetAllianceName(m_vAllianceName);
            alliance.SetAllianceDescription(m_vAllianceDescription);
            alliance.SetAllianceType(m_vAllianceType);
            alliance.SetRequiredScore(m_vRequiredScore);
            alliance.SetAllianceBadgeData(m_vAllianceBadgeData);
            //alliance.SetAllianceOrigin(m_vAllianceOrigin);
            alliance.SetWarFrequency(m_vWarFrequency);

            //Set player clan
            //ObjectManager.OnlinePlayers.TryGetValue(p.Client, out player);
            level.GetPlayerAvatar().SetAllianceId(alliance.GetAllianceId());
            AllianceMemberEntry member = new AllianceMemberEntry(level.GetPlayerAvatar().GetId());
            member.SetRole(2);
            alliance.AddAllianceMember(member);

            var joinAllianceCommand = new JoinAllianceCommand();
            joinAllianceCommand.SetAlliance(alliance);
            var availableServerCommandMessage = new AvailableServerCommandMessage(this.Client);
            availableServerCommandMessage.SetCommandId(1);
            availableServerCommandMessage.SetCommand(joinAllianceCommand);
            PacketManager.ProcessOutgoingPacket(availableServerCommandMessage);
        }
    }
}
