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
    class CSVTable
    {
        private List<CSVRow> m_vCSVRows;
        private List<string> m_vColumnHeaders;
        private List<string> m_vColumnTypes;
        private List<CSVColumn> m_vCSVColumns;

        public CSVTable(string filePath)
        {
            m_vCSVRows = new List<CSVRow>();
            m_vColumnHeaders = new List<string>();
            m_vColumnTypes = new List<string>();
            m_vCSVColumns = new List<CSVColumn>();

            using (var sr = new StreamReader(filePath))
            {
                var columns = sr.ReadLine().Replace("\"", "").Replace(" ", "").Split(',');
                foreach (var column in columns)
                {
                    m_vColumnHeaders.Add(column);
                    m_vCSVColumns.Add(new CSVColumn());
                }

                var types = sr.ReadLine().Replace("\"", "").Split(',');
                foreach (var type in types)
                {
                    m_vColumnTypes.Add(type);
                }

                while (!sr.EndOfStream)
                {
                    var values = sr.ReadLine().Replace("\"", "").Split(',');
                    
                    if(values[0] != String.Empty)
                    {
                        CreateRow();
                    }

                    for (int i = 0; i < m_vColumnHeaders.Count;i++ )
                    {
                        m_vCSVColumns[i].Add(values[i]);
                    }
                }
            }
        }

        public int GetArraySizeAt(CSVRow row, int columnIndex)
        {
            int rowIndex = m_vCSVRows.IndexOf(row);
            if (rowIndex == -1)
                return 0;
            CSVColumn c = m_vCSVColumns[columnIndex];
            int nextOffset = 0;
            if(rowIndex + 1 >= m_vCSVRows.Count)
            {
                nextOffset = c.GetSize();
            }
            else
            {
                CSVRow nextRow = m_vCSVRows[rowIndex + 1];
                nextOffset = nextRow.GetRowOffset();
            }
            int currentOffset = row.GetRowOffset();
            return c.GetArraySize(currentOffset, nextOffset);
        }

        public int GetRowCount()
        {
            return m_vCSVRows.Count;
        }

        public CSVRow GetRowAt(int index)
        {
            return m_vCSVRows[index];
        }

        public int GetColumnIndexByName(string name)
        {
            return m_vColumnHeaders.IndexOf(name);
        }

        public string GetColumnName(int index)
        {
            return m_vColumnHeaders[index];
        }

        public string GetValueAt(int column, int row)
        {
            return m_vCSVColumns[column].Get(row);
        }

        public string GetValue(string name, int level)
        {
            int index = m_vColumnHeaders.IndexOf(name);
            return GetValueAt(index, level);
        }

        public void CreateRow()
        {
            new CSVRow(this);
        }

        public void AddRow(CSVRow row)
        {
            m_vCSVRows.Add(row);
        }

        public int GetColumnRowCount()
        {
            int result = 0;
            if (m_vCSVColumns.Count > 0)
                result = m_vCSVColumns[0].GetSize();
            return result;
        }
    }

}
