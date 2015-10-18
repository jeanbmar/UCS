using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UCS.Core;
using UCS.Helpers;
using UCS.Logic;
using UCS.Network;

namespace UCS.PacketProcessing
{
    //14325
    class AskForAvatarProfileMessage : Message
    {
        private long m_vAvatarId;
        private long m_vCurrentHomeId;

        public AskForAvatarProfileMessage(Client client, BinaryReader br) : base(client, br)
        {
            //Empty pack
        }

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                m_vAvatarId = br.ReadInt64WithEndian();
                m_vCurrentHomeId = br.ReadInt64WithEndian();
            }
        }

        public override void Process(Level level)
        {
            var targetLevel = ResourcesManager.GetPlayer(m_vAvatarId);
            if (targetLevel != null)
            {
                targetLevel.Tick();
                var p = new AvatarProfileMessage(this.Client);
                p.SetLevel(targetLevel);
                PacketManager.ProcessOutgoingPacket(p);
            } 
        }
    }
}
