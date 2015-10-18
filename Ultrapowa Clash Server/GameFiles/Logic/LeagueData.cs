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
    class LeagueData : Data
    {

        public LeagueData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, this.GetType(), row);
        }

        public String TID { get; set; }
        public String TIDShort { get; set; }
        public String IconSWF { get; set; }
        public String IconExportName { get; set; }
        public String LeagueBannerIcon { get; set; }
        public String LeagueBannerIconNum { get; set; }
        public int GoldReward { get; set; }
        public int ElixirReward { get; set; }
        public int DarkElixirReward { get; set; }
        public int PlacementLimitLow { get; set; }
        public int PlacementLimitHigh { get; set; }
        public int DemoteLimit { get; set; }
        public int PromoteLimit { get; set; }
        public int BucketPlacementRangeLow { get; set; }
        public int BucketPlacementRangeHigh { get; set; }
        public int BucketPlacementSoftLimit { get; set; }
        public int BucketPlacementHardLimit { get; set; }
        public bool IgnoredByServer { get; set; }
        public bool DemoteEnabled { get; set; }
        public bool PromoteEnabled { get; set; }
    }
}
