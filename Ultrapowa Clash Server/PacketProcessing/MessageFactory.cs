using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using UCS.Logic;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Command list: LogicCommand::createCommand
    static class MessageFactory
    {
        private static Dictionary<int, Type> m_vMessages;

        static MessageFactory()
        {
            m_vMessages = new Dictionary<int, Type>();
            m_vMessages.Add(10101, typeof(LoginMessage));
            m_vMessages.Add(10108, typeof(KeepAliveMessage));
            m_vMessages.Add(10212, typeof(ChangeAvatarNameMessage));
            m_vMessages.Add(14101, typeof(GoHomeMessage));
            m_vMessages.Add(14102, typeof(ExecuteCommandsMessage));
            m_vMessages.Add(14113, typeof(VisitHomeMessage));
            m_vMessages.Add(14134, typeof(AttackNpcMessage));
            //m_vMessages.Add(14136, );//Edit Clan Messages
            m_vMessages.Add(14301, typeof(CreateAllianceMessage));
            m_vMessages.Add(14302, typeof(AskForAllianceDataMessage));
            m_vMessages.Add(14303, typeof(AskForJoinableAlliancesListMessage));
            m_vMessages.Add(14305, typeof(JoinAllianceMessage));
            //m_vMessages.Add(14306, );//Promote
            m_vMessages.Add(14308, typeof(LeaveAllianceMessage));
            m_vMessages.Add(14315, typeof(ChatToAllianceStreamMessage));
            m_vMessages.Add(14324, typeof(SearchAlliancesMessage));
            m_vMessages.Add(14325, typeof(AskForAvatarProfileMessage));
            m_vMessages.Add(14715, typeof(SendGlobalChatLineMessage));
        }

        public static object Read(Client c, BinaryReader br, int packetType)
        {
            if (m_vMessages.ContainsKey(packetType))
            {
                return Activator.CreateInstance(m_vMessages[packetType], c, br);
            }
            else
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("U");
                Console.ResetColor();
                Console.WriteLine("] " + packetType.ToString() + " Unhandled Message (ignored)");
                return null;
            }
        }
    }
}
