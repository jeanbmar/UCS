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
    class Deco : GameObject
    {
        private Level m_vLevel;

        public override int ClassId
        {
            get { return 6; }
        }

        public Deco(Data data, Level l) : base(data, l)
        {
            m_vLevel = l;
        }

        public DecoData GetDecoData()
        {
            return (DecoData)GetData();
        }

        public new JObject Save(JObject jsonObject)
        {
            base.Save(jsonObject);
            return jsonObject;
        }

        public new void Load(JObject jsonObject)
        {
            base.Load(jsonObject);
        }
    }
}
