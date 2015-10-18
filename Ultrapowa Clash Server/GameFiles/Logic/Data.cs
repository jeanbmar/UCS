using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Configuration;
using System.Reflection;
using UCS.PacketProcessing;
using UCS.Core;
using UCS.GameFiles;
using UCS.Logic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UCS.GameFiles
{
    class Data
    {
        private int m_vGlobalID;
        protected CSVRow m_vCSVRow;
        protected DataTable m_vDataTable;

        public Data(CSVRow row, DataTable dt)
        {
            m_vCSVRow = row;
            m_vDataTable = dt;
            m_vGlobalID = GlobalID.CreateGlobalID(dt.GetTableIndex() + 1, dt.GetItemCount());
        }

        public string GetName()
        {
            return m_vCSVRow.GetName();
        }

        public int GetDataType()
        {
            return m_vDataTable.GetTableIndex();
        }

        public int GetGlobalID()
        {
            return m_vGlobalID;
        }

        public int GetInstanceID()
        {
            return GlobalID.GetInstanceID(m_vGlobalID);
        }

        public void LoadData(Data obj, Type objectType, CSVRow row)
        {
            foreach (PropertyInfo prop in objectType.GetProperties())
            {
                if (prop.PropertyType.IsGenericType)
                {
                    var listType = typeof(List<>);
                    var genericArgs = prop.PropertyType.GetGenericArguments();
                    var concreteType = listType.MakeGenericType(genericArgs);
                    var newList = Activator.CreateInstance(concreteType);

                    var add = concreteType.GetMethod("Add");

                    String indexerName = ((DefaultMemberAttribute)newList.GetType().GetCustomAttributes(typeof(DefaultMemberAttribute), true)[0]).MemberName;
                    PropertyInfo indexerProp = newList.GetType().GetProperty(indexerName);

                    for (int i = row.GetRowOffset(); i < (row.GetRowOffset() + row.GetArraySize(prop.Name)); i++)
                    {
                        string v = row.GetValue(prop.Name, i - row.GetRowOffset());
                        if (v == String.Empty && i != row.GetRowOffset())
                            v = indexerProp.GetValue(newList, new Object[] { i - row.GetRowOffset() - 1 }).ToString();
                        
                        if(v == String.Empty)
                        {
                            object o = genericArgs[0].IsValueType ? Activator.CreateInstance(genericArgs[0]) : "";
                            add.Invoke(newList, new[] { o });
                        }
                        else
                            add.Invoke(newList, new[] { Convert.ChangeType(v, genericArgs[0]) });
                    }

                    prop.SetValue(obj, newList);
                }
                else
                {
                    if (row.GetValue(prop.Name, 0) == string.Empty)
                        prop.SetValue(obj, null, null);
                    else
                        prop.SetValue(obj, Convert.ChangeType(row.GetValue(prop.Name, 0), prop.PropertyType), null);
                }
            }
        }
    }
}
