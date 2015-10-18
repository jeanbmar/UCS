using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using UCS.Helpers;
using UCS.Logic;
using UCS.Core;

namespace UCS.PacketProcessing
{
    //Packet 14102
    class ExecuteCommandsMessage : Message
    {
        private byte[] m_vCommands;

        public ExecuteCommandsMessage(Client client, BinaryReader br) : base (client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                //Console.WriteLine(base.ToHexString());
                Unknown1 = br.ReadUInt32WithEndian();
                Unknown2 = br.ReadUInt32WithEndian();
                NumberOfCommands = br.ReadUInt32WithEndian();

                if (NumberOfCommands > 0)
                {
                    m_vCommands = br.ReadBytes(GetLength() - 12);
                }
            }
        }

        public byte[] NestedCommands
        {
            get { return m_vCommands; }
        }

        public uint Unknown1 { get; set; } //00 00 2B D8 some sort of server tick
        public uint Unknown2 { get; set; } // 01 EB 30 36 some sort of server tick or checksum
        public uint NumberOfCommands { get; set; }
        
        public override void Process(Level level)
        {
            try
            {
                level.Tick();

                if (NumberOfCommands > 0)
                {
                    using (var br = new BinaryReader(new MemoryStream(NestedCommands)))
                    {
                        for (int i = 0; i < NumberOfCommands; i++)
                        {
                            object obj = CommandFactory.Read(br);
                            if (obj != null)
                            {
                                string player = "";
                                if (level != null)
                                    player += " (" + level.GetPlayerAvatar().GetId() + ", " + level.GetPlayerAvatar().GetAvatarName() + ")";
                                Debugger.WriteLine("\t" + obj.GetType().Name + player);
                                ((Command)obj).Execute(level);
                                //Debugger.WriteLine("finished processing of command " + obj.GetType().Name + player);
                            }
                            else
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Debugger.WriteLine("Exception occurred during command processing." + ex.ToString());
                Console.ResetColor();
            }
        }
    }
}
