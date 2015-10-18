using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using UCS.Core;

namespace UCS.GameFiles
{
    class DecoData : Data
    {

        public DecoData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, this.GetType(), row);
        }

        public int GetBuildCost()
        {
            return BuildCost;
        }

        public ResourceData GetBuildResource()
        {
            return ObjectManager.DataTables.GetResourceByName(BuildResource);
        }

        public int GetSellPrice()
        {
            int calculation = (int)(((long)BuildCost * (long)1717986919) >> 32);
            return ((calculation >> 2) + (calculation >> 31));
        }

        public String TID { get; set; }
        public String InfoTID { get; set; }
        public String SWF { get; set; }
        public String ExportName { get; set; }
        public String ExportNameConstruction { get; set; }
        public String BuildResource { get; set; }
        public int BuildCost { get; set; }
        public int RequiredExpLevel { get; set; }
        public int MaxCount { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public String Icon { get; set; }
        public String ExportNameBase { get; set; }
        public String ExportNameBaseNpc { get; set; }
        public String ExportNameBaseWar { get; set; }

    }
}
