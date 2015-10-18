using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using UCS.Logic;
using UCS.Helpers;
using UCS.GameFiles;
using UCS.Core;

namespace UCS.PacketProcessing
{
    //Commande 503
    class SellBuildingCommand : Command
    {
        private int m_vBuildingId;


        public SellBuildingCommand(BinaryReader br)
        {
            m_vBuildingId = br.ReadInt32WithEndian();
            br.ReadUInt32WithEndian();
        }

        public override void Execute(Level level)
        {
            ClientAvatar ca = level.GetPlayerAvatar();
            GameObject go = level.GameObjectManager.GetGameObjectByID(m_vBuildingId);

            if(go != null)
            {
                if(go.ClassId == 4)
                {
                    Trap t = (Trap)go;
                    int upgradeLevel = t.GetUpgradeLevel();
                    var rd = t.GetTrapData().GetBuildResource(upgradeLevel);
                    int sellPrice = t.GetTrapData().GetSellPrice(upgradeLevel);
                    ca.CommodityCountChangeHelper(0, rd, sellPrice);
                    level.GameObjectManager.RemoveGameObject(t);
                }
                else if(go.ClassId == 6)
                {
                    Deco d = (Deco)go;
                    var rd = d.GetDecoData().GetBuildResource();
                    int sellPrice = d.GetDecoData().GetSellPrice();
                    if(rd.PremiumCurrency)
                    {
                        ca.SetDiamonds(ca.GetDiamonds() + sellPrice);
                    }
                    else
                    {
                        ca.CommodityCountChangeHelper(0, rd, sellPrice);
                    }
                    level.GameObjectManager.RemoveGameObject(d);
                }
                else
                {
                    //TODO BUILDING
                    /*
                    Building b = (Building)go;
                    level.GameObjectManager.RemoveGameObject(b);
                     * */
                }
            }  
        }
    }
}
