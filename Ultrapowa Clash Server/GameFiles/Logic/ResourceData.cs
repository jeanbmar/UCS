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
    class ResourceData : Data
    {

        public ResourceData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, this.GetType(), row); 
        }

        public String TID { get; set; }
        public String SWF { get; set; }
        public String CollectEffect { get; set; }
        public String ResourceIconExportName { get; set; }
        public String StealEffect { get; set; }
        public Boolean PremiumCurrency { get; set; }
        public String HudInstanceName { get; set; }
        public String CapFullTID { get; set; }
        public int TextRed { get; set; }
        public int TextGreen { get; set; }
        public int TextBlue { get; set; }
        public String WarRefResource { get; set; }
    }
}
