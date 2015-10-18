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
            m_vCommands.Add(0x0001, typeof(JoinAllianceCommand));
            m_vCommands.Add(0x0002, typeof(LeaveAllianceCommand));
            m_vCommands.Add(0x01F4, typeof(BuyBuildingCommand));
            m_vCommands.Add(0x01F5, typeof(MoveBuildingCommand));
            m_vCommands.Add(0x01F6, typeof(UpgradeBuildingCommand));
            m_vCommands.Add(0x01F7, typeof(SellBuildingCommand));
            m_vCommands.Add(0x01F8, typeof(SpeedUpConstructionCommand));
            m_vCommands.Add(0x01F9, typeof(CancelConstructionCommand));
            m_vCommands.Add(0x01FA, typeof(CollectResourcesCommand));
            //m_vCommands.Add(0x01FB, typeof(ClearObstacle));
            m_vCommands.Add(0x01FC, typeof(TrainUnitCommand));
            m_vCommands.Add(0x01FD, typeof(CancelUnitProductionCommand));
            m_vCommands.Add(0x01FE, typeof(BuyTrapCommand));
            //m_vCommands.Add(0x01FF, typeof(RequestAllianceUnits));
            m_vCommands.Add(0x0200, typeof(BuyDecoCommand));
            m_vCommands.Add(0x0201, typeof(SpeedUpTrainingCommand));
            m_vCommands.Add(0x0202, typeof(SpeedUpClearingCommand));
            //m_vCommands.Add(0x0203, typeof(CancelUpgradeUnit));
            m_vCommands.Add(0x0204, typeof(UpgradeUnitCommand));
            m_vCommands.Add(0x0205, typeof(SpeedUpUpgradeUnitCommand));
            m_vCommands.Add(0x0206, typeof(BuyResourcesCommand));
            //m_vCommands.Add(0x0207, typeof(MissionProgress));
            //m_vCommands.Add(0x0208, typeof(UnlockBuilding));
            m_vCommands.Add(0x0209, typeof(FreeWorkerCommand));
            //m_vCommands.Add(0x020A, typeof(BuyShield));
            //m_vCommands.Add(0x020B, typeof(ClaimAchievementReward));
            //m_vCommands.Add(0x020C, typeof(ToggleAttackMode));
            //m_vCommands.Add(0x020D, typeof(LoadTurret));
            //m_vCommands.Add(0x020E, typeof(BoostBuilding));
            m_vCommands.Add(0x020F, typeof(UpgradeHeroCommand));
            m_vCommands.Add(0x0210, typeof(SpeedUpHeroUpgradeCommand));
            //m_vCommands.Add(0x0211, typeof(ToggleHeroSleep));
            //m_vCommands.Add(0x0212, typeof(SpeedUpHeroHealth));
            m_vCommands.Add(0x0213, typeof(CancelHeroUpgradeCommand));
            //m_vCommands.Add(0x0214, typeof(NewShopItemsSeen));
            m_vCommands.Add(0x0215, typeof(MoveMultipleBuildingsCommand));
            m_vCommands.Add(0x0219, typeof(SendAllianceMailCommand));
            m_vCommands.Add(0x021B, typeof(Unknown539Command));
            m_vCommands.Add(0x021F, typeof(KickAllianceMemberCommand));
            m_vCommands.Add(0x0225, typeof(UpgradeMultipleBuildingsCommand));
            m_vCommands.Add(0x0226, typeof(RemoveUnitsCommand));
            m_vCommands.Add(0x0258, typeof(PlaceAttackerCommand));
            m_vCommands.Add(0x025C, typeof(CastSpellCommand));
            m_vCommands.Add(0x02BC, typeof(SearchOpponentCommand));
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
                Console.Write("\t");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("Unhandled");
                Console.ResetColor();
                Console.WriteLine(" Command " + cm.ToString() + " (ignored)");
                return null;
            }
        }
    }
}
