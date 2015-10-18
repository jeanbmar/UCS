using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Configuration;
using UCS.PacketProcessing;
using UCS.Core;
using UCS.GameFiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UCS.Logic
{
    class Component
    {
        private bool m_vIsEnabled;//a1 + 8
        private GameObject m_vParentGameObject;

        public virtual int Type
        {
            get { return -1; }
        }

        public Component()
        {
            //do not modify
        }

        public Component(GameObject go)
        {
            m_vIsEnabled = true;
            m_vParentGameObject = go;
        }

        public bool IsEnabled()
        {
            return m_vIsEnabled;
        }

        public GameObject GetParent()
        {
            return m_vParentGameObject;
        }

        public virtual void Load(JObject jsonObject)
        {
        
        }

        public virtual JObject Save(JObject jsonObject)
        {
            return jsonObject;
        }

        public void SetEnabled(bool status)
        {
            m_vIsEnabled = status;
        }

        public virtual void Tick()
        {

        }
    }
}
