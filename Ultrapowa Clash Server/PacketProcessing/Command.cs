using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    class Command
    {
        public Command() { }

        public virtual void Execute(Level level)
        {
        }

        public virtual byte[] Encode()
        {

            return new List<byte>().ToArray();
        }
    }
}
