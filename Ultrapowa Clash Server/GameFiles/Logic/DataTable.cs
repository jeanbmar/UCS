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
    class DataTable
    {
        protected List<Data> m_vData;
        protected int m_vIndex;

        public DataTable()
        {
            m_vIndex = 0;
            m_vData = new List<Data>();
        }

        public DataTable(CSVTable table, int index)
        {
            m_vIndex = index;
            m_vData = new List<Data>();

            for(int i=0;i<table.GetRowCount();i++)
            {
                var row = table.GetRowAt(i);
                var data = CreateItem(row);
                m_vData.Add(data);
            }
        }

        public Data CreateItem(CSVRow row)
        {
            Data d = new Data(row, this);
            switch (m_vIndex)
            {
                case 0:
                    d = new BuildingData(row, this);
                    break;
                case 2:
                    d = new ResourceData(row, this);
                    break;
                case 3:
                    d = new CharacterData(row, this);
                    break;
                case 7:
                    d = new ObstacleData(row, this);
                    break;
                case 10:
                    d = new ExperienceLevelData(row, this);
                    break;
                case 11:
                    d = new TrapData(row, this);
                    break;
                case 12:
                    d = new LeagueData(row, this);
                    break;
                case 13:
                    d = new GlobalData(row, this);
                    break;
                case 14:
                    d = new TownhallLevelData(row, this);
                    break;
                case 16:
                    d = new NpcData(row, this);
                    break;
                case 17:
                    d = new DecoData(row, this);
                    break;
                case 19:
                    d = new ShieldData(row, this);
                    break;
                case 22:
                    d = new AchievementData(row, this);
                    break;
                case 23:
                    d = new Data(row, this);
                    break;
                case 24:
                    d = new Data(row, this);
                    break;
                case 25:
                    d = new SpellData(row, this);
                    break;
                case 27:
                    d = new HeroData(row, this);
                    break;
                /*case 28:
                    d = new WarData(dic);
                    break;*/
                default:
                    break;
            }
            return d;
        }

        public int GetTableIndex()
        {
            return m_vIndex;
        }

        public Data GetItemAt(int index)
        {
            return m_vData[index];
        }

        public Data GetItemById(int id)
        {
            int instanceId = GlobalID.GetInstanceID(id);
            return m_vData[instanceId];
        }

        public int GetItemCount()
        {
            return m_vData.Count;
        }

        public Data GetDataByName(string name)
        {
            return m_vData.Find(d => d.GetName() == name);
        }

    }

}
