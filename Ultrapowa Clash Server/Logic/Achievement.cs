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
    class Achievement 
    {
        private const int m_vType = 0x015EF3C0;

        public Achievement() { 
            //Deserialization
        }

        public Achievement(int index)
        {
            //this.Name = ObjectManager.AchievementsData.GetData(index, 0).Name;
            this.Index = index;
            this.Unlocked = false;
            this.Value = 0;
        }

        public string Name { get; set; }
        public int Id 
        {
            get { return m_vType + this.Index; } 
        }
        public int Index { get; set; }
        public bool Unlocked { get; set; }
        public int Value { get; set; }
    }
}
