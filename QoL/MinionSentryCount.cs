using System;
using PluginLoader;
using Terraria;
using Terraria.ID;

namespace VisxositysPluginSuite
{
    public class MinionSentryCount : MarshalByRefObject, IPluginChatCommand
    {
        public bool OnChatCommand(string command, string[] args)
        {
            if (command == "minioncount" || command == "mc" || command == "minionc" || command == "mcount")
            {
                Player player = Main.player[Main.myPlayer];

                int minionCount = 0;
                int sentryCount = 0;
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.active && proj.owner == player.whoAmI && proj.minion)
                        minionCount++;
                    if (proj.active && proj.owner == player.whoAmI && proj.sentry)
                        sentryCount++;
                }

                Main.NewText(
                    string.Format("Minions: {0} of {1} | Sentries: {2} of {3}",
                        minionCount, player.maxMinions, sentryCount, player.maxTurrets),
                    135, 206, 250 // LightSkyBlue RGB
                );
                return true;
            }
            return false;
        }
    }
}
