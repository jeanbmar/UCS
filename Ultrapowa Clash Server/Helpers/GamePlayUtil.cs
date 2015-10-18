using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UCS.GameFiles;
using UCS.Core;
using UCS.Logic;

namespace UCS.Helpers
{
    static class GamePlayUtil
    {
        public static int GetResourceDiamondCost(int resourceCount, ResourceData resourceData)
        {
            var globals = ObjectManager.DataTables.GetGlobals();
            return globals.GetResourceDiamondCost(resourceCount, resourceData);
        }

        public static int GetSpeedUpCost(int seconds)
        {
            var globals = ObjectManager.DataTables.GetGlobals();
            return globals.GetSpeedUpCost(seconds);
        }

        public static int CalculateResourceCost(int sup, int inf, int supCost, int infCost, int amount)
        {
            return (int)Math.Round((long)(supCost - infCost) * (long)(amount - inf) / (sup - inf * 1.0)) + infCost;
        }

        public static int CalculateSpeedUpCost(int sup, int inf, int supCost, int infCost, int amount)
        {
            return (int)Math.Round((long)(supCost - infCost) * (long)(amount - inf) / (sup - inf * 1.0)) + infCost;
        }
    }

}
