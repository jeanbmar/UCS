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
    class Building : ConstructionItem
    {
        public override int ClassId
        {
            get { return 0; }
        }

        public Building(Data data, Level level) : base(data, level) 
        {
            this.Locked = GetBuildingData().Locked;
            AddComponent(new HitpointComponent());
            if (GetBuildingData().IsHeroBarrack)
            {
                HeroData hd = ObjectManager.DataTables.GetHeroByName(GetBuildingData().HeroType);
                AddComponent(new HeroBaseComponent(this, hd));
            }
            if (GetBuildingData().UpgradesUnits)
                AddComponent(new UnitUpgradeComponent(this));
            if (GetBuildingData().UnitProduction[0] > 0)
                AddComponent(new UnitProductionComponent(this));
            if (GetBuildingData().HousingSpace[0] > 0)
            {
                if (GetBuildingData().Bunker)
                    AddComponent(new BunkerComponent());
                else
                    AddComponent(new UnitStorageComponent(this, 0));
            }
            if (GetBuildingData().Damage[0] > 0)
                AddComponent(new CombatComponent());
            if (GetBuildingData().ProducesResource != String.Empty)
                AddComponent(new ResourceProductionComponent());
            if (GetBuildingData().MaxStoredGold[0] > 0 ||
                GetBuildingData().MaxStoredElixir[0] > 0 ||
                GetBuildingData().MaxStoredDarkElixir[0] > 0 ||
                GetBuildingData().MaxStoredWarGold[0] > 0 ||
                GetBuildingData().MaxStoredWarElixir[0] > 0 ||
                GetBuildingData().MaxStoredWarDarkElixir[0] > 0)
                AddComponent(new ResourceStorageComponent(this));
        }

        public BuildingData GetBuildingData()
        {
            return (BuildingData)GetData();
        }
    }

}
