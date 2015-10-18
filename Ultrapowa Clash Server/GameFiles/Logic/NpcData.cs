using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace UCS.GameFiles
{
    class NpcData : Data
    {

        public NpcData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, this.GetType(), row);
        }

        public String MapInstanceName { get; set; }
        public String MapDependencies { get; set; }
        public String TID { get; set; }
        public int ExpLevel { get; set; }
        public String UnitType { get; set; }
        public int UnitCount { get; set; }
        public String LevelFile { get; set; }
        public int Gold { get; set; }
        public int Elixir { get; set; }
        public bool AlwaysUnlocked { get; set; }

    }
}
