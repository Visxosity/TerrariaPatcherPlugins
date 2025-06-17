using System;
using PluginLoader;
using Terraria;

namespace VisxositysPluginSuite
{
    public class AdjustableSummons : MarshalByRefObject, IPluginPlayerUpdateArmorSets, IPluginChatCommand
    {
        private int minions;
        private int turrets;

        public AdjustableSummons()
        {
            if (!int.TryParse(IniAPI.ReadIni("AdjustableSummons", "ExtraMinions", "0", writeIt: true), out minions)) minions = 0;
            if (!int.TryParse(IniAPI.ReadIni("AdjustableSummons", "ExtraTurrets", "0", writeIt: true), out turrets)) turrets = 0;
        }

        public void OnPlayerUpdateArmorSets(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                player.maxMinions += minions;
                player.maxTurrets += turrets;
            }
        }

        public bool OnChatCommand(string command, string[] args)
        {
            if (command == "minionplus")
            {
                int amount = 1;
                if (args.Length > 0)
                {
                    int parsed;
                    if (int.TryParse(args[0], out parsed))
                        amount = parsed;
                }
                minions = Math.Min(minions + amount, 100);
                IniAPI.WriteIni("AdjustableSummons", "ExtraMinions", minions.ToString());
                Main.NewText("Extra Minions: " + minions, 255, 235, 150);
                return true;
            }
            if (command == "minionminus")
            {
                int amount = 1;
                if (args.Length > 0)
                {
                    int parsed;
                    if (int.TryParse(args[0], out parsed))
                        amount = parsed;
                }
                minions = Math.Max(minions - amount, 0);
                IniAPI.WriteIni("AdjustableSummons", "ExtraMinions", minions.ToString());
                Main.NewText("Extra Minions: " + minions, 255, 235, 150);
                return true;
            }
            if (command == "sentryplus")
            {
                int amount = 1;
                if (args.Length > 0)
                {
                    int parsed;
                    if (int.TryParse(args[0], out parsed))
                        amount = parsed;
                }
                turrets = Math.Min(turrets + amount, 100);
                IniAPI.WriteIni("AdjustableSummons", "ExtraTurrets", turrets.ToString());
                Main.NewText("Extra Sentries: " + turrets, 255, 235, 150);
                return true;
            }
            if (command == "sentryminus")
            {
                int amount = 1;
                if (args.Length > 0)
                {
                    int parsed;
                    if (int.TryParse(args[0], out parsed))
                        amount = parsed;
                }
                turrets = Math.Max(turrets - amount, 0);
                IniAPI.WriteIni("AdjustableSummons", "ExtraTurrets", turrets.ToString());
                Main.NewText("Extra Sentries: " + turrets, 255, 235, 150);
                return true;
            }
            return false;
        }
    }
}