using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UCS.PacketProcessing;
using UCS.Core;
using UCS.GameFiles;

namespace UCS.Logic
{
    class GameObjectManager
    {
        private List<List<GameObject>> m_vGameObjects;
        private List<int> m_vGameObjectsIndex;
        private ComponentManager m_vComponentManager;
        private Level m_vLevel;

        public GameObjectManager(Level l) 
        {
            m_vLevel = l;
            m_vGameObjects = new List<List<GameObject>>();
            m_vGameObjectsIndex = new List<int>();
            for (int i = 0; i < 7; i++)
            {
                m_vGameObjects.Add(new List<GameObject>());
                m_vGameObjectsIndex.Add(0);
            }
            m_vComponentManager = new ComponentManager(m_vLevel);
        }

        public ComponentManager GetComponentManager()
        {
            return m_vComponentManager;
        }

        public GameObject GetGameObjectByID(int id)
        {
            int classId = GlobalID.GetClassID(id) - 500;
            return m_vGameObjects[classId].Find(g => g.GlobalId == id);
        }

        public List<GameObject> GetGameObjects(int id)
        {
            return m_vGameObjects[id];
        }

        public void AddGameObject(GameObject go)
        {
            go.GlobalId = GenerateGameObjectGlobalId(go);
            if (go.ClassId == 0)
            {
                Building b = (Building)go;
                BuildingData bd = b.GetBuildingData();
                if (bd.IsWorkerBuilding())
                {
                    m_vLevel.WorkerManager.IncreaseWorkerCount();
                }
            }
            m_vGameObjects[go.ClassId].Add(go);
        }

        public void RemoveGameObject(GameObject go)
        {
            m_vGameObjects[go.ClassId].Remove(go);
            if(go.ClassId == 0)
            {
                Building b = (Building)go;
                BuildingData bd = b.GetBuildingData();
                if(bd.IsWorkerBuilding())
                {
                    m_vLevel.WorkerManager.DecreaseWorkerCount();
                }
            }
            RemoveGameObjectReferences(go);
        }

        public void RemoveGameObjectReferences(GameObject go)
        {
            m_vComponentManager.RemoveGameObjectReferences(go);
        }

        private int GenerateGameObjectGlobalId(GameObject go)
        {
            int index = m_vGameObjectsIndex[go.ClassId];
            m_vGameObjectsIndex[go.ClassId]++;
            return GlobalID.CreateGlobalID(go.ClassId + 500, index); 
        }

        public void Tick()
        {
            m_vComponentManager.Tick();
            foreach(var l in m_vGameObjects)
            {
                foreach (GameObject go in l)
                    go.Tick();
            }
        }

        public JObject Save()
        {
            JObject jsonData = new JObject();

            //Buildings
            JArray jsonBuildingsArray = new JArray();
            foreach (var go in m_vGameObjects[0])
            {
                Building b = (Building)go;
                JObject jsonObject = new JObject();
                jsonObject.Add("data", b.GetBuildingData().GetGlobalID());
                b.Save(jsonObject);
                jsonBuildingsArray.Add(jsonObject);
            }
            jsonData.Add("buildings",jsonBuildingsArray);

            //Traps
            JArray jsonTrapsArray = new JArray();
            foreach (var go in m_vGameObjects[4])
            {
                Trap t = (Trap)go;
                JObject jsonObject = new JObject();
                jsonObject.Add("data", t.GetTrapData().GetGlobalID());
                t.Save(jsonObject);
                jsonTrapsArray.Add(jsonObject);
            }
            jsonData.Add("traps",jsonTrapsArray);

            //Decos
            JArray jsonDecosArray = new JArray();
            foreach (var go in m_vGameObjects[6])
            {
                Deco d = (Deco)go;
                JObject jsonObject = new JObject();
                jsonObject.Add("data", d.GetDecoData().GetGlobalID());
                d.Save(jsonObject);
                jsonDecosArray.Add(jsonObject);
            }
            jsonData.Add("decos", jsonDecosArray);

            return jsonData;
        }

        public void Load(JObject jsonObject)
        {
            JArray jsonBuildings = (JArray)jsonObject["buildings"];
            foreach(JObject jsonBuilding in jsonBuildings)
            {
                BuildingData bd = (BuildingData)ObjectManager.DataTables.GetDataById(jsonBuilding["data"].ToObject<int>());
                Building b = new Building(bd, m_vLevel);
                AddGameObject(b);
                b.Load(jsonBuilding);
            }

            JArray jsonTraps = (JArray)jsonObject["traps"];
            foreach (JObject jsonTrap in jsonTraps)
            {
                TrapData td = (TrapData)ObjectManager.DataTables.GetDataById(jsonTrap["data"].ToObject<int>());
                Trap t = new Trap(td, m_vLevel);
                AddGameObject(t);
                t.Load(jsonTrap);
            }

            JArray jsonDecos = (JArray)jsonObject["decos"];
            foreach (JObject jsonDeco in jsonDecos)
            {
                DecoData dd = (DecoData)ObjectManager.DataTables.GetDataById(jsonDeco["data"].ToObject<int>());
                Deco d = new Deco(dd, m_vLevel);
                AddGameObject(d);
                d.Load(jsonDeco);
            }
        }
    }
}
