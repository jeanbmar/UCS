using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;
using UCS.Core;
using UCS.Network;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    class GameOpCommand
    {
        private byte m_vRequiredAccountPrivileges;

        public GameOpCommand() { }

        public virtual void Execute(Level level) { }

        public byte GetRequiredAccountPrivileges()
        {
            return m_vRequiredAccountPrivileges;
        }

        public void SetRequiredAccountPrivileges(byte level)
        {
            m_vRequiredAccountPrivileges = level;
        }

        public void SendCommandFailedMessage(Client c)
        {
            //Debugger.WriteLine("GameOp command failed. Insufficient privileges.");
            var p = new GlobalChatLineMessage(c);
            p.SetChatMessage("GameOp command failed. Insufficient privileges.");
            p.SetPlayerId(0);
            p.SetPlayerName("Ultrapowa Clash Server");
            PacketManager.ProcessOutgoingPacket(p);
        }
    }
}
