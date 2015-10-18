using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Logic;
using UCS.Core;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Packet 24133
    class NpcDataMessage : Message
    {
        public NpcDataMessage(Client client, Level level, AttackNpcMessage cnam) : base (client)
        {
            SetMessageType(24133);

            this.Player = level;

            JsonBase = ObjectManager.NpcLevels[(int)cnam.LevelId - 0x01036640];
          
            LevelId = cnam.LevelId;
        }

        public override void Encode()
        {
            List<Byte> data = new List<Byte>();

            data.AddInt32(0);
            data.AddInt32(JsonBase.Length);
            data.AddRange(System.Text.Encoding.ASCII.GetBytes(JsonBase));
            data.AddRange(Player.GetPlayerAvatar().Encode());
            data.AddInt32(0);
            data.AddInt32(LevelId);

            SetData(data.ToArray());
        }

        public String JsonBase { get; set; }
        public int LevelId { get; set; }
        public Level Player { get; set; }
    }
}
