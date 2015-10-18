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
using UCS.Network;

namespace UCS.PacketProcessing
{
    class RenameAvatarGameOpCommand : GameOpCommand
    {
        private string[] m_vArgs;

        public RenameAvatarGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(1);
        }

        public override void Execute(Level level)
        {
            if(level.GetAccountPrivileges() >= GetRequiredAccountPrivileges())
            {
                if(m_vArgs.Length >= 3)
                {
                    try
                    {
                        long id = Convert.ToInt64(m_vArgs[1]);
                        var l = ResourcesManager.GetPlayer(id, true);
                        if(l != null)
                        {
                            l.GetPlayerAvatar().SetName(m_vArgs[2]);
                            if (ResourcesManager.IsPlayerOnline(l))
                            {
                                var p = new AvatarNameChangeOkMessage(l.GetClient());
                                p.SetAvatarName(m_vArgs[2]);
                                PacketManager.ProcessOutgoingPacket(p);
                            } 
                        }
                        else
                        {
                            Debugger.WriteLine("RenameAvatar failed: id " + id + " not found");
                        }
                    }
                    catch(Exception ex)
                    { 
                        Debugger.WriteLine("RenameAvatar failed with error: " + ex.ToString()); 
                    }
                }
            }
            else
            {
                SendCommandFailedMessage(level.GetClient());
            }
        }
    }
}
