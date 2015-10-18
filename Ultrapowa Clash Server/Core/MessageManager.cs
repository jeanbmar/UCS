using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Reflection;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using UCS.Network;
using UCS.Logic;
using UCS.PacketProcessing;
using UCS.Helpers;

namespace UCS.Core
{
    class MessageManager
    {
        private static ConcurrentQueue<Message> m_vPackets;
        private static EventWaitHandle m_vWaitHandle = new AutoResetEvent(false);

        private bool m_vIsRunning;

        private delegate void PacketProcessingDelegate();

        public MessageManager()
        {
            m_vPackets = new ConcurrentQueue<Message>();
            m_vIsRunning = false;
        }

        public void Start()
        {
            PacketProcessingDelegate packetProcessing = new PacketProcessingDelegate(PacketProcessing);
            packetProcessing.BeginInvoke(null, null);

            m_vIsRunning = true;

            Console.WriteLine("Message Manager started");
        }

        private void PacketProcessing()
        {
            while(m_vIsRunning)
            {
                m_vWaitHandle.WaitOne();

                Message p;
                while (m_vPackets.TryDequeue(out p))
                {
                    Level pl = p.Client.GetLevel();
                    string player = "";
                    if (pl != null)
                        player += " (" + pl.GetPlayerAvatar().GetId() + ", " + pl.GetPlayerAvatar().GetAvatarName() + ")";
                    try
                    {
                        Debugger.WriteLine("[R] " + p.GetMessageType() + " " + p.GetType().Name + player);
                        p.Decode();
                        p.Process(pl);
                        //Debugger.WriteLine("finished processing of message " + p.GetType().Name + player);
                    }
                    catch(Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Debugger.WriteLine("An exception occured during processing of message " + p.GetType().Name + player, ex);
                        Console.ResetColor();
                    }
                }
            }
        }

        public static void ProcessPacket(Message p)
        {
            m_vPackets.Enqueue(p);
            m_vWaitHandle.Set();
        }
    }
}
