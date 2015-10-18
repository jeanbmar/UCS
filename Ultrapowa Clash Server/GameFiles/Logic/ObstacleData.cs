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
    class ObstacleData : Data
    {

        public ObstacleData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, this.GetType(), row);
        }

        public ResourceData GetClearingResource()
        {
            return ObjectManager.DataTables.GetResourceByName(ClearResource);
        }

        public String TID { get; set; }
        public String SWF { get; set; }
        public String ExportName { get; set; }
        public String ExportNameBase { get; set; }
        public String ExportNameBaseNpc { get; set; }
        public int ClearTimeSeconds { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public String Resource { get; set; }
        public bool Passable { get; set; }
        public String ClearResource { get; set; }
        public int ClearCost { get; set; }
        public String LootResource { get; set; }
        public int LootCount { get; set; }
        public String ClearEffect { get; set; }
        public String PickUpEffect { get; set; }
        public int RespawnWeight { get; set; }
        public bool IsTombstone { get; set; }
        public int TombGroup { get; set; }
        public int LootMultiplierForVersion2 { get; set; }
        public int AppearancePeriodHours { get; set; }
        public int MinRespawnTimeHours { get; set; }

    }
}
