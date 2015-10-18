using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Configuration;
using System.Windows;
using UCS.PacketProcessing;
using UCS.Core;
using UCS.GameFiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UCS.Logic
{
    class GameObject
    {
        private Data m_vData;
        public int X { get; set; }
        public int Y { get; set; }
        public int GlobalId { get; set; }//a1 + 4
        public virtual int ClassId 
        {
            get { return -1; } 
        }

        private List<Component> m_vComponents;
        private Level m_vLevel; //a1 + 8

        public GameObject(Data data, Level level)
        {
            m_vLevel = level;
            m_vData = data;
            m_vComponents = new List<Component>();
            for (int i = 0; i < 11; i++)
                m_vComponents.Add(new Component());
        }

        public void AddComponent(Component c)
        {
            if(m_vComponents[c.Type].Type != -1)
            {
                //ignore, component already set
            }
            else
            {
                m_vLevel.GetComponentManager().AddComponent(c);
                m_vComponents[c.Type] = c;
            }
        }

        public Component GetComponent(int index, bool test)
        {
            Component result = null;
            if (!test || m_vComponents[index].IsEnabled())
                result = m_vComponents[index];
            return result;
        }

        public Data GetData()
        {
            return m_vData;
        }

        public Level GetLevel()
        {
            return m_vLevel;
        }

        public Vector GetPosition()
        {
            return new Vector(this.X, this.Y);
        }

        public virtual bool IsHero()
        {
            return false;
        }

        public void SetPositionXY(int newX, int newY)
        {
            this.X = newX;
            this.Y = newY;
        }

        public virtual void Tick() { }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("x", this.X);
            jsonObject.Add("y", this.Y);
            foreach (var c in m_vComponents)
                c.Save(jsonObject);
            return jsonObject;
        }

        public void Load(JObject jsonObject)
        {
            this.X = jsonObject["x"].ToObject<int>();
            this.Y = jsonObject["y"].ToObject<int>();
            foreach (var c in m_vComponents)
                c.Load(jsonObject);
        }
    }
}
