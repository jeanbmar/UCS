using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Logic;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Packet 24715
    class GlobalChatLineMessage : Message
    {
        private string m_vMessage;
        private long m_vHomeId;
        private long m_vCurrentHomeId;
        private string m_vPlayerName;
        private int m_vLeagueId;

        public GlobalChatLineMessage(Client client) : base(client)
        {
            SetMessageType(24715);

            m_vMessage = "default";
            m_vPlayerName = "default";
            m_vHomeId = 1;
            m_vCurrentHomeId = 1;
        }

        public override void Encode()
        {
            List<Byte> pack = new List<Byte>();

            pack.AddString(m_vMessage);
            pack.AddString(m_vPlayerName);
            pack.AddInt32(0x05);
            pack.AddInt32(m_vLeagueId);
            pack.AddInt64(m_vHomeId);
            pack.AddInt64(m_vCurrentHomeId);
            pack.AddInt32(0);

            SetData(pack.ToArray());
        }

        public void SetChatMessage(string message)
        {
            m_vMessage = message;
        }

        public void SetPlayerId(long id)
        {
            m_vHomeId = id;
            m_vCurrentHomeId = id;
        }

        public void SetPlayerName(string name)
        {
            m_vPlayerName = name;
        }

        public void SetLeagueId(int leagueId)
        {
            m_vLeagueId = leagueId;
        }
    }
}
