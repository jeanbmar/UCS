using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UCS.Helpers;
using UCS.Logic;
using UCS.Network;

namespace UCS.PacketProcessing
{
    //10212
    class ChangeAvatarNameMessage : Message
    {
        public ChangeAvatarNameMessage(Client client, BinaryReader br) : base (client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                PlayerName = br.ReadScString();
                Unknown1 = br.ReadByte();
            }
        }

        public int PlayerNameLength { get; set; }
        public String PlayerName { get; set; }
        public byte Unknown1 { get; set; }

        public override void Process(Level level)
        {
            level.GetPlayerAvatar().SetName(this.PlayerName);
            var p = new AvatarNameChangeOkMessage(this.Client);
            p.SetAvatarName(level.GetPlayerAvatar().GetAvatarName());
            PacketManager.ProcessOutgoingPacket(p);
        }
    }
}
