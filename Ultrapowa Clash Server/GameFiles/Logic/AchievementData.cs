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
    class AchievementData : Data
    {
        public AchievementData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, this.GetType(), row);
        }

        public int Level { get; set; }
        public String TID { get; set; }
        public String InfoTID { get; set; }
        public String Action { get; set; }
        public int ActionCount { get; set; }
        public String ActionData { get; set; }
        public int ExpReward { get; set; }
        public int DiamondReward { get; set; }
        public String IconSWF { get; set; }
        public String IconExportName { get; set; }
        public String CompletedTID { get; set; }
        public bool ShowValue { get; set; }
        public String AndroidID { get; set; }
    }
}
