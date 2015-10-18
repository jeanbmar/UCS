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
    class GlobalData : Data
    {

        public GlobalData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, this.GetType(), row);
        }

        public int NumberValue { get; set; }
        public bool BooleanValue { get; set; }
        public string TextValue { get; set; }
        public int NumberArray { get; set; }
        public String StringArray { get; set; }
        public String AltStringArray { get; set; }

    }
}
