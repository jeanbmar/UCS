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
    class SetPrivilegesGameOpCommand : GameOpCommand
    {
        private string[] m_vArgs;

        public SetPrivilegesGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(4);
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
                        byte accountPrivileges = Convert.ToByte(m_vArgs[2]);
                        var l = ResourcesManager.GetPlayer(id);
                        if (accountPrivileges < level.GetAccountPrivileges())
                        {
                            if (l != null)
                            {
                                l.SetAccountPrivileges(accountPrivileges);
                            }
                            else
                            {
                                Debugger.WriteLine("SetPrivileges failed: id " + id + " not found");
                            }
                        }
                        else
                        {
                            Debugger.WriteLine("SetPrivileges failed: target privileges too high");
                        }
                    }
                    catch(Exception ex)
                    {
                        Debugger.WriteLine("SetPrivileges failed with error: " + ex.ToString()); 
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
