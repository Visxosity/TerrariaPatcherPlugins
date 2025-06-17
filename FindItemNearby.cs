using System;
using PluginLoader;
using Terraria;

namespace VisxositysPluginSuite
{
    public class FindItemNearby : MarshalByRefObject, IPluginChatCommand
    {
        public bool OnChatCommand(string command, string[] args)
        {
            // Use "deep" instead of "partial" in commands
            if ((command == "find" || command == "finddeep" || command == "locate" || command == "locatedeep") && args.Length > 0)
            {
                string search = string.Join(" ", args).ToLower();
                int found = 0;
                bool deep = command.Contains("deep");
                bool quickStackRange = command.StartsWith("find");
                Player player = Main.player[Main.myPlayer];

                for (int i = 0; i < Main.maxChests; i++)
                {
                    Chest chest = Main.chest[i];
                    if (chest == null) continue;

                    int chestTileX = chest.x;
                    int chestTileY = chest.y + 1;

                    // Only check quick stack range if needed (25 tiles)
                    float dx = chestTileX - (player.Center.X / 16f);
                    float dy = chestTileY - (player.Center.Y / 16f);
                    if (quickStackRange && (Math.Abs(dx) > 25 || Math.Abs(dy) > 25))
                        continue;

                    for (int j = 0; j < Chest.maxItems; j++)
                    {
                        Item item = chest.item[j];
                        if (item != null && item.stack > 0)
                        {
                            string itemName = item.Name.ToLower();
                            bool match = deep ? itemName.Contains(search) : itemName == search;
                            if (match)
                            {
                                if (quickStackRange)
                                {
                                    // Distance in full tiles, rounded down
                                    int xTiles = (int)Math.Floor(Math.Abs(dx));
                                    int yTiles = (int)Math.Floor(Math.Abs(dy));
                                    string xDir = dx > 0 ? "right" : dx < 0 ? "left" : "same X";
                                    string yDir = dy > 0 ? "below" : dy < 0 ? "above" : "same Y";

                                    Main.NewText(
                                        string.Format(
                                            "Found {0} x{1} ({2} tiles {3}, {4} tiles {5}, slot {6})",
                                            item.Name, item.stack,
                                            xTiles, xDir,
                                            yTiles, yDir,
                                            j + 1
                                        ),
                                        255, 255, 0
                                    );
                                }
                                else
                                {
                                    // Compass: east/west of world center
                                    int worldCenterX = Main.maxTilesX / 2;
                                    int feetEastWest = (chestTileX - worldCenterX) * 2;
                                    string ewDir = feetEastWest > 0 ? "east" : feetEastWest < 0 ? "west" : "at world center";
                                    int ewFeet = Math.Abs(feetEastWest);

                                    // Depth meter: above/below world surface
                                    int feetDepth = (int)((chestTileY - Main.worldSurface) * 2);
                                    string depthDir = feetDepth > 0 ? "below" : feetDepth < 0 ? "above" : "at surface";
                                    int depthFeet = Math.Abs(feetDepth);

                                    Main.NewText(
                                        string.Format(
                                            "Found {0} x{1} ({2} feet {3} of world center, {4} feet {5} surface, slot {6})",
                                            item.Name, item.stack,
                                            ewFeet, ewDir,
                                            depthFeet, depthDir,
                                            j + 1
                                        ),
                                        255, 255, 0
                                    );
                                }
                                found++;
                            }
                        }
                    }
                }

                if (found == 0)
                    Main.NewText("No matching items found in chests.", 255, 100, 100);

                return true;
            }
            return false;
        }
    }
}