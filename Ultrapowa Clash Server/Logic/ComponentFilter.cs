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
    class ComponentFilter : GameObjectFilter
    {
        public int Type;//a1 + 20

        public ComponentFilter(int type) : base()
        {
            Type = type;
        }

        public override bool IsComponentFilter()
        {
            return true;
        }

        public new bool TestGameObject(GameObject go)
        {
            bool result = false;
            Component c = go.GetComponent(Type, true);
            if(c != null)
            {
                result = base.TestGameObject(go);
            }
            return result;
        }

        public bool TestComponent(Component c)
        {
            GameObject go = c.GetParent();
            return TestGameObject(go);
        }
    }
}
