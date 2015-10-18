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
    class CSVRow
    {
        private CSVTable m_vCSVTable;
        private int m_vRowStart;

        public CSVRow(CSVTable table)
        {
            m_vCSVTable = table;
            m_vRowStart = m_vCSVTable.GetColumnRowCount();
            m_vCSVTable.AddRow(this);
        }

        public int GetArraySize(string name)
        {
            int columnIndex = m_vCSVTable.GetColumnIndexByName(name);
            int result = 0;
            if (columnIndex != -1)
                result = m_vCSVTable.GetArraySizeAt(this, columnIndex);
            return result;

        }

        public int GetRowOffset()
        {
            return m_vRowStart;
        }

        public string GetName()
        {
            return m_vCSVTable.GetValueAt(0, m_vRowStart);
        }

        public string GetValue(string name, int level)
        {
            return m_vCSVTable.GetValue(name, level + m_vRowStart);
        }
    }

}
