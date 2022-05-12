﻿using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;

namespace stardew_access.Features
{
    public class ReadTile
    {
        private bool isBusy; // To pause execution of run method between fixed intervals
        private int delay; // Length of each interval (in ms)
        private bool shouldPause; // To pause the execution
        private Vector2 prevTile;

        public ReadTile()
        {
            isBusy = false;
            delay = 100;
        }

        public void update()
        {
            if (this.isBusy)
                return;

            if (this.shouldPause)
                return;

            if (!MainClass.Config.ReadTile)
                return;

            this.isBusy = true;
            this.run();
            Task.Delay(delay).ContinueWith(_ => { this.isBusy = false; });
        }

        /// <summary>
        /// Pauses the read tile for the provided time.
        /// </summary>
        /// <param name="time">The amount of time we want to pause the execution (in ms).<br/>Default is 2500 (2.5s).</param>
        public void pause(int time = 2500)
        {
            if (!MainClass.Config.ReadTileAllowPausing)
                return;

            this.shouldPause = true;
            Task.Delay(time).ContinueWith(_ => { this.shouldPause = false; });
        }

        public void run(bool manuallyTriggered = false, bool playersPosition = false)
        {
            try
            {
                Vector2 tile;

                #region Get Tile
                int x, y;
                if (!playersPosition)
                {
                    // Grab tile
                    tile = CurrentPlayer.FacingTile;
                }
                else
                {
                    // Player's standing tile
                    tile = CurrentPlayer.Position;
                }
                x = (int)tile.X;
                y = (int)tile.Y;
                #endregion

                if (Context.IsPlayerFree)
                {
                    if (!manuallyTriggered && prevTile != tile)
                    {
                        if (MainClass.ScreenReader != null)
                            MainClass.ScreenReader.PrevTextTile = " ";
                    }

                    bool isColliding = TileInfo.isCollidingAtTile(x, y);

                    (string? name, string? category) info = TileInfo.getNameWithCategoryNameAtTile(tile);

                    #region Narrate toSpeak
                    if (info.name != null)
                        if (MainClass.ScreenReader != null)
                            if (manuallyTriggered)
                                MainClass.ScreenReader.Say($"{info.name}, Category: {info.category}", true);
                            else
                                MainClass.ScreenReader.SayWithTileQuery(info.name, x, y, true);
                    #endregion

                    #region Play colliding sound effect
                    if (isColliding && prevTile != tile)
                    {
                        Game1.playSound("colliding");
                    }
                    #endregion

                    if (!manuallyTriggered)
                        prevTile = tile;
                }

            }
            catch (Exception e)
            {
                MainClass.ErrorLog($"Error in Read Tile:\n{e.Message}\n{e.StackTrace}");
            }
        }
    }
}
