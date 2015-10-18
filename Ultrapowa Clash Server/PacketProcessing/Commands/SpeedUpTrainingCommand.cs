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
    //Commande 0x201
    class SpeedUpTrainingCommand : Command
    {
        private int m_vBuildingId;

        public SpeedUpTrainingCommand(BinaryReader br)
        {
            m_vBuildingId = br.ReadInt32WithEndian();
            br.ReadInt32WithEndian();
        }

        //00 00 02 01 1D CD 65 10 00 00 38 A6

        public override void Execute(Level level)
        {
            ClientAvatar ca = level.GetPlayerAvatar();
            GameObject go = level.GameObjectManager.GetGameObjectByID(m_vBuildingId);
            
            if(go != null)
            {
                if(go.ClassId == 0)
                {
                    Building b = (Building)go;
                    UnitProductionComponent upc = b.GetUnitProductionComponent();
                    if(upc != null)
                    {
                        int totalRemainingTime = upc.GetTotalRemainingSeconds();
                        int cost = GamePlayUtil.GetSpeedUpCost(totalRemainingTime);
                        if(upc.IsSpellForge())
                        {
                            int multiplier = ObjectManager.DataTables.GetGlobals().GetGlobalData("SPELL_SPEED_UP_COST_MULTIPLIER").NumberValue;
                            cost = (int)(((long)cost * (long)multiplier * (long)1374389535) >> 32);
                            cost = Math.Max((cost >> 5) + (cost >> 31), 1);
                        }
                        if (ca.HasEnoughDiamonds(cost))
                        {
                            if (upc.HasHousingSpaceForSpeedUp())
                            {
                                ca.UseDiamonds(cost);
                                upc.SpeedUp();
                            }
                        }
                    }
                }
            }
        }
    }
}
