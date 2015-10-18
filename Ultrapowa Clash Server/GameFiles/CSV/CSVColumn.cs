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
    class CSVColumn
    {
        private List<string> m_vValues;

        public CSVColumn()
        {
            m_vValues = new List<string>();
        }

        public void Add(string value)
        {
            //if (value == string.Empty)
            //    m_vValues.Add(m_vValues.Last());
            //else
            m_vValues.Add(value);
        }

        public string Get(int row)
        {
            return m_vValues[row];
        }

        public int GetSize()
        {
            return m_vValues.Count;
        }

        public int GetArraySize(int currentOffset, int nextOffset)
        {
            return nextOffset - currentOffset;
        }
    }

}
