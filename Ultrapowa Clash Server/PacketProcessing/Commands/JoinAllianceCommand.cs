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

namespace UCS.PacketProcessing
{
    //Commande 0x001
    class JoinAllianceCommand : Command
    {
        private Alliance m_vAlliance;

        public JoinAllianceCommand()
        {
            //AvailableServerCommandMessage
        }

        public JoinAllianceCommand(BinaryReader br)
        {
            br.ReadInt64WithEndian();
            br.ReadScString();
            br.ReadInt32WithEndian();
            br.ReadByte();
            br.ReadInt32WithEndian();
            br.ReadInt32WithEndian();
            br.ReadInt32WithEndian();
        }

        public void SetAlliance(Alliance alliance)
        {
            m_vAlliance = alliance;
        }

        public override byte[] Encode()
        {
            List<Byte> data = new List<Byte>();
            data.AddRange(m_vAlliance.EncodeHeader());
            return data.ToArray();
        }

        //00 00 00 46 00 03 46 FE 
        //00 00 00 0B 
        //4C 61 20 54 65 61 6D 20 54 44 41 
        //5E 00 2C 5A 
        //00 
        //00 00 00 02 
        //00 00 00 01 
        //00 00 1C 35
    }
}
