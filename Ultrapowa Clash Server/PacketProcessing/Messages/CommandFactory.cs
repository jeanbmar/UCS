using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using UCS.Logic;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Command list: LogicCommand::createCommand
    static class CommandFactory
    {
        private static Dictionary<uint, Type> m_vCommands;

        static CommandFactory()
        {
            m_vCommands = new Dictionary<uint, Type>();
            m_vCommands.Add(0x01F4, typeof(BuyBuilding));
            m_vCommands.Add(0x01F5, typeof(MoveBuilding));
            m_vCommands.Add(0x01F6, typeof(UpgradeBuilding));
            m_vCommands.Add(0x01F7, typeof(SellBuilding));
            m_vCommands.Add(0x01F8, typeof(SpeedUpConstruction));
            m_vCommands.Add(0x01F9, typeof(CancelConstruction));
            m_vCommands.Add(0x01FA, typeof(CollectResources));
            //m_vCommands.Add(0x01FB, typeof(ClearObstacle));
            m_vCommands.Add(0x01FC, typeof(TrainUnit));
            m_vCommands.Add(0x01FD, typeof(CancelUnitProduction));
            m_vCommands.Add(0x01FE, typeof(BuyTrap));
            //m_vCommands.Add(0x01FF, typeof(RequestAllianceUnits));
            m_vCommands.Add(0x0200, typeof(BuyDeco));
            m_vCommands.Add(0x0201, typeof(SpeedUpTraining));
            //m_vCommands.Add(0x0202, typeof(SpeedUpClearing));
            //m_vCommands.Add(0x0203, typeof(CancelUpgradeUnit));
            m_vCommands.Add(0x0204, typeof(UpgradeUnit));
            m_vCommands.Add(0x0205, typeof(SpeedUpUpgradeUnit));
            m_vCommands.Add(0x0206, typeof(BuyResources));
            //m_vCommands.Add(0x0207, typeof(MissionProgress));
            //m_vCommands.Add(0x0208, typeof(UnlockBuilding));
            m_vCommands.Add(0x0209, typeof(FreeWorker));
            //m_vCommands.Add(0x020A, typeof(BuyShield));
            //m_vCommands.Add(0x020B, typeof(ClaimAchievementReward));
            //m_vCommands.Add(0x020C, typeof(ToggleAttackMode));
            //m_vCommands.Add(0x020D, typeof(LoadTurret));
            //m_vCommands.Add(0x020E, typeof(BoostBuilding));
            m_vCommands.Add(0x020F, typeof(UpgradeHero));
            m_vCommands.Add(0x0210, typeof(SpeedUpHeroUpgrade));
            //m_vCommands.Add(0x0211, typeof(ToggleHeroSleep));
            //m_vCommands.Add(0x0212, typeof(SpeedUpHeroHealth));
            //m_vCommands.Add(0x0213, typeof(CancelHeroUpgrade));
            //m_vCommands.Add(0x0214, typeof(NewShopItemsSeen));
            m_vCommands.Add(0x0215, typeof(MoveWallRange));
            m_vCommands.Add(0x021B, typeof(Unknown539));
            m_vCommands.Add(0x0225, typeof(UpgradeWallRange));
            m_vCommands.Add(0x0226, typeof(RemoveUnits));
            m_vCommands.Add(0x0258, typeof(PlaceAttacker));
            m_vCommands.Add(0x025C, typeof(CastSpell));
        }

        public static object Read(BinaryReader br)
        {
            uint cm = br.ReadUInt32WithEndian();
            if (m_vCommands.ContainsKey(cm))
            {
                return Activator.CreateInstance(m_vCommands[cm], br);
            }
            else
            {
                Console.WriteLine("Unhandled command " + cm.ToString() + ". Ignored.");
                return null;
            }
        }

        public static void Execute(object obj, Level pl)
        {
            if (obj != null)
            {
                MethodInfo mti = obj.GetType().GetMethod("Execute");
                object[] param = new object[] { pl };
                mti.Invoke(obj, param);
            }
        }
    }
}
