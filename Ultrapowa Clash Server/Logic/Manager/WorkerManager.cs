using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.ComponentModel;
using Newtonsoft.Json;
using UCS.PacketProcessing;
using UCS.Core;
using UCS.GameFiles;

namespace UCS.Logic
{
    class WorkerManager
    {
        private List<GameObject> m_vGameObjectReferences;
        private int m_vWorkerCount;

        public WorkerManager() 
        {
            m_vGameObjectReferences = new List<GameObject>();
            m_vWorkerCount = 0;
        }

        public int GetFreeWorkers()
        {
            return m_vWorkerCount - m_vGameObjectReferences.Count;
        }

        public int GetTotalWorkers()
        {
            return m_vWorkerCount;
        }

        public void AllocateWorker(GameObject go)
        {
            if(m_vGameObjectReferences.IndexOf(go) == -1)
            {
                m_vGameObjectReferences.Add(go);
            }
        }

        public void DeallocateWorker(GameObject go)
        {
            if (m_vGameObjectReferences.IndexOf(go) != -1)
            {
                m_vGameObjectReferences.Remove(go);
            }
        }

        public void RemoveGameObjectReferences(GameObject go)
        {

        }

        public void IncreaseWorkerCount()
        {
            m_vWorkerCount++;
        }

        public void DecreaseWorkerCount()
        {
            m_vWorkerCount--;
        }

        public GameObject GetShortestTaskGO()
        {
            GameObject shortestTaskGO = null;
            int shortestGOTime = 0;
            int currentGOTime;

            foreach(var go in m_vGameObjectReferences)
            {
                currentGOTime = -1;
                if(go.ClassId == 3)
                {
                    Obstacle o = (Obstacle)go;
                    if(o.IsClearingOnGoing())
                        currentGOTime = o.GetRemainingClearingTime();
                }
                else
                {
                    ConstructionItem c = (ConstructionItem)go;
                    if(c.IsConstructing())
                    {
                        currentGOTime = c.GetRemainingConstructionTime();
                    }
                    else
                    {
                        var hero = c.GetHeroBaseComponent();
                        if(hero != null)
                        {
                            if(hero.IsUpgrading())
                            {
                                currentGOTime = hero.GetRemainingUpgradeSeconds();
                            }
                        }
                    }
                }
                if(shortestTaskGO == null)
                {
                    if(currentGOTime > -1)
                    {
                        shortestTaskGO = go;
                        shortestGOTime = currentGOTime;
                    }  
                }
                else if (currentGOTime > -1)
                {
                    if (currentGOTime < shortestGOTime)
                    {
                        shortestGOTime = currentGOTime;
                        shortestTaskGO = go;
                    }
                }
            }
            return shortestTaskGO;
        }

        public int GetFinishTaskOfOneWorkerCost()
        {
            return 0;
        }

        public void FinishTaskOfOneWorker()
        {
            GameObject go = GetShortestTaskGO();
            if(go != null)
            {
                if (go.ClassId == 3)
                {
                    Obstacle o = (Obstacle)go;
                    if (o.IsClearingOnGoing())
                        o.SpeedUpClearing();
                }
                else
                {
                    ConstructionItem b = (ConstructionItem)go;
                    if (b.IsConstructing())
                        b.SpeedUpConstruction();
                    else
                    {
                        var hero = b.GetHeroBaseComponent();
                        if (hero != null)
                            hero.SpeedUpUpgrade();
                    }
                }
            }
        }
    }
}
