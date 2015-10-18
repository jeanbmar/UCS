using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using UCS.PacketProcessing;
using UCS.Core;
using UCS.GameFiles;
using UCS.Helpers;

namespace UCS.Logic
{
    class UnitSlot 
    {
        public CombatItemData UnitData;//a1 + 4
        public int Level;//a1 + 8
        public int Count;//a1 + 12

        public UnitSlot(CombatItemData cd, int level, int count)
        {
            UnitData = cd;
            Level = level;
            Count = count;
        }

        public void Decode(BinaryReader br)
        {
            UnitData = (CombatItemData)br.ReadDataReference();
            Level = br.ReadInt32WithEndian();
            Count = br.ReadInt32WithEndian();
        }
    }
}
