using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    //Packet 24111
    class AvatarNameChangeOkMessage : Message
    {
        private string m_vAvatarName;
        private int m_vServerCommandType;

        public AvatarNameChangeOkMessage(Client client) : base(client)
        {
            SetMessageType(24111);

            m_vServerCommandType = 0x03;
            m_vAvatarName = "Megapumba";
        }

        public string GetAvatarName()
        {
            return m_vAvatarName;
        }

        public void SetAvatarName(string name)

        {
            m_vAvatarName = name;
        }

        public override void Encode()
        {
            List<Byte> pack = new List<Byte>();

            pack.AddInt32(m_vServerCommandType);
            pack.AddString(m_vAvatarName);
            pack.AddInt32(1);
            pack.AddInt32(-1);

            SetData(pack.ToArray());
        }

    }
}
