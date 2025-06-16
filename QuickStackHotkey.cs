using System;
using Microsoft.Xna.Framework.Input;
using PluginLoader;
using Terraria;

namespace VisxositysPluginSuite
{
    public class QuickStackHotkey : MarshalByRefObject, IPluginChatCommand
    {
        private static readonly string[] Messages = new[]
        {
            "Quick Stacked to Nearby Chests!",
            "Your inventory feels lighter.",
            "Items zoomed into chests!",
            "Stack attack complete!",
            "Chest magic at work!",
            "Inventory organized!",
            "That was quick... stack!",
            "Stacked with style!",
            "All sorted and stacked!",
            "Another successful quick stack!"
        };

        private static readonly Random rng = new Random();

        public bool OnChatCommand(string command, string[] args)
        {
            if (command == "quickstack")
            {
                Player player = Main.player[Main.myPlayer];

                // Record inventory stack counts before quick stacking
                int[] before = new int[player.inventory.Length];
                for (int i = 0; i < player.inventory.Length; i++)
                    before[i] = player.inventory[i] != null ? player.inventory[i].stack : 0;

                player.QuickStackAllChests();

                // Check if any stack count decreased (item was moved)
                bool quickStacked = false;
                for (int i = 0; i < player.inventory.Length; i++)
                {
                    int after = player.inventory[i] != null ? player.inventory[i].stack : 0;
                    if (after < before[i])
                    {
                        quickStacked = true;
                        break;
                    }
                }

                if (quickStacked)
                {
                    string msg = Messages[rng.Next(Messages.Length)];
                    Main.NewText(msg, 135, 206, 250);
                }
                else
                {
                    Main.NewText("No items could be quick stacked.", 255, 180, 80);
                }
                return true;
            }
            return false;
        }
    }
}