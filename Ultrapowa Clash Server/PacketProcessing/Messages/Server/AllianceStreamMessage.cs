using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using UCS.Logic;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Packet 24311
    class AllianceStreamMessage : Message
    {
        private Alliance m_vAlliance;

        public AllianceStreamMessage(Client client, Alliance alliance)
            : base(client)
        {
            SetMessageType(24311);
            m_vAlliance = alliance;
        }

        public override void Encode()
        {
            List<Byte> pack = new List<Byte>();

            List<StreamEntry> chatMessages = m_vAlliance.GetChatMessages().ToList();//avoid concurrent access issues

            pack.AddInt32(chatMessages.Count);
            foreach (var chatMessage in chatMessages)
            {
                pack.AddRange(chatMessage.Encode());
            }
            
            SetData(pack.ToArray());
        }
    }
}
