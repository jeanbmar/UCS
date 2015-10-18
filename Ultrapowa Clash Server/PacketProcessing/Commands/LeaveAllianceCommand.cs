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
    //Commande 0x0002
    class LeaveAllianceCommand : Command
    {
        private Alliance m_vAlliance;
        private int m_vReason;

        public LeaveAllianceCommand()
        {
            //AvailableServerCommandMessage
        }

        public LeaveAllianceCommand(BinaryReader br)
        {
            br.ReadInt64WithEndian();
            br.ReadInt32WithEndian();
            br.ReadInt32WithEndian();
        }

        public void SetAlliance(Alliance alliance)
        {
            m_vAlliance = alliance;
        }

        public void SetReason(int reason)
        {
            m_vReason = reason;
        }

        public override byte[] Encode()
        {
            List<Byte> data = new List<Byte>();
            data.AddInt64(m_vAlliance.GetAllianceId());
            data.AddInt32(m_vReason);
            data.AddInt32(-1);
            return data.ToArray();
        }

        //00 00 00 3B 00 0A 40 1E 
        //00 00 00 01 ////reason? 1= leave, 2=kick
        //00 00 07 3A
    }
}
