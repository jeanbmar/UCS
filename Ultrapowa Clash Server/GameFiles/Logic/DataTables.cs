using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using UCS.Logic;

namespace UCS.GameFiles
{
    class DataTables
    {
        private List<DataTable> m_vDataTables;

        public DataTables()
        {
            m_vDataTables = new List<DataTable>();
            for (int i = 0; i < 28; i++)
                m_vDataTables.Add(new DataTable());
        }

        public void InitDataTable(CSVTable t, int index)
        {
            if (index == 13)
                m_vDataTables[index] = new Globals(t, index);
            else
                m_vDataTables[index] = new DataTable(t, index);
        }   

        public CharacterData GetCharacterByName(string name)
        {
            DataTable dt = m_vDataTables[3];
            return (CharacterData)dt.GetDataByName(name);
        }

        public Data GetDataById(int id)
        {
            int classId = GlobalID.GetClassID(id) - 1;
            DataTable dt = m_vDataTables[classId];
            return dt.GetItemById(id);
        }

        public Globals GetGlobals()
        {
            return (Globals)m_vDataTables[13];
        }

        public HeroData GetHeroByName(string name)
        {
            DataTable dt = m_vDataTables[27];
            return (HeroData)dt.GetDataByName(name);
        }

        public ResourceData GetResourceByName(string name)
        {
            DataTable dt = m_vDataTables[2];
            return (ResourceData)dt.GetDataByName(name);
        }

        public DataTable GetTable(int i)
        {
            return m_vDataTables[i];
        }

    }

}
