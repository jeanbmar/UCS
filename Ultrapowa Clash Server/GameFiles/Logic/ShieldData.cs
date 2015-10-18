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
    class ShieldData : Data
    {

        public ShieldData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, this.GetType(), row);
        }
        
        public String TID { get; set; }
        public String InfoTID { get; set; }
        public int TimeH { get; set; }
        public int Diamonds { get; set; }
        public String IconSWF { get; set; }
        public String IconExportName { get; set; }
        public int CooldownS { get; set; }
        public int LockedAboveScore { get; set; }

    }
}
