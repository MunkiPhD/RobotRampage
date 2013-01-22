using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace RobotRampage {
    static class Player {

        #region Declarations
        public static Sprite BaseSprite;
        public static Sprite TurretSprite;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture">The sprite sheet</param>
        /// <param name="baseInitialFrame">The base frame for the tank</param>
        /// <param name="baseFrameCount">The base frame animation count</param>
        /// <param name="turretInitialFrame">The base frame for the turret</param>
        /// <param name="turretFrameCount">The animation count for the turret</param>
        /// <param name="worldLocation">Location on the world map</param>
        public static void Initialize(Texture2D texture, Rectangle baseInitialFrame, int baseFrameCount,
            Rectangle turretInitialFrame, int turretFrameCount, Vector2 worldLocation) {

            int frameWidth = baseInitialFrame.Width;
            int frameHeight = baseInitialFrame.Height;
            BaseSprite = new Sprite(worldLocation, texture, baseInitialFrame, Vector2.Zero);
            BaseSprite.BoundingXPadding = 4;
            BaseSprite.BoundingYPadding = 4;
            BaseSprite.AnimateWhenStopped = false;
            // i think it should use framewidth * x, not frameheight * x
            for(int x = 1; x < baseFrameCount; x++)
                BaseSprite.AddFrame(new Rectangle(baseInitialFrame.X + (frameHeight * x), baseInitialFrame.Y, frameWidth, frameHeight));

            TurretSprite = new Sprite(worldLocation, texture, turretInitialFrame, Vector2.Zero);
            // i think this should be turrentSprite, not basesprite
            for(int x =1; x <turretFrameCount; x++)
                BaseSprite.AddFrame(new Rectangle(turretInitialFrame.X + (frameHeight * x), turretInitialFrame.Y, frameWidth, frameHeight));

        }



        /// <summary>
        /// Update!
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime) {
            BaseSprite.Update(gameTime);
            TurretSprite.WorldLocation = BaseSprite.WorldLocation;
        }




        public static void Draw(SpriteBatch spriteBatch) {
            BaseSprite.Draw(spriteBatch);
            TurretSprite.Draw(spriteBatch);
        }
    } // end class
} // end namespace
