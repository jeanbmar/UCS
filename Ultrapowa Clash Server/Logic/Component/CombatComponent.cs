using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Configuration;
using UCS.PacketProcessing;
using UCS.Core;
using UCS.GameFiles;
using Newtonsoft.Json;

namespace UCS.Logic
{
    class CombatComponent : Component
    {
        private const int m_vType = 0x01AB3F00;

        public CombatComponent()
        {
            //Deserialization
        }

        public override int Type
        {
            get { return 1; }
        }
    }
}
