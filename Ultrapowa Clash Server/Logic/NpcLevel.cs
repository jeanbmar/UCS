using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.ComponentModel;
using Newtonsoft.Json;
using UCS.PacketProcessing;
using UCS.Core;
using UCS.GameFiles;

namespace UCS.Logic
{
    class NpcLevel 
    {
        private const int m_vType = 0x01036640;

        public NpcLevel() { 
            //Deserialization
        }

        public NpcLevel(int index)
        {
            //this.Name = ObjectManager.NpcsData.GetData(index, 0).Name;
            this.Index = index;
            this.Stars = 0;
            this.LootedGold = 0;
            this.LootedElixir = 0;
        }

        public string Name { get; set; }
        public int Id 
        {
            get { return m_vType + this.Index; } 
        }
        public int Index { get; set; }
        public int Stars { get; set; }
        public int LootedGold { get; set; }
        public int LootedElixir { get; set; }
    }
}
