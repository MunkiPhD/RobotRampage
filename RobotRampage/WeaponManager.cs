using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RobotRampage {
    static class WeaponManager {

        #region Declarations
        static public List<Particle> Shots = new List<Particle>();
        static public Texture2D Texture;
        static public Rectangle shotRectangle = new Rectangle(0, 128, 32, 32);
        static public float WeaponSpeed = 600f;
        static private float _shotTimer = 0f;
        static private float _shotMinTimer = 0.15f;
        #endregion

        #region Properties
        static public float WeaponFireDelay {
            get { return _shotMinTimer; }
        }

        static public bool CanFireWeapon {
            get { return (_shotTimer >= WeaponFireDelay); }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        static public void Update(GameTime gameTime) {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _shotTimer += elapsed;

            for (int x = Shots.Count - 1; x >= 0; x--) {
                Shots[x].Update(gameTime);

                if(Shots[x].Expired)
                    Shots.RemoveAt(x);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        static public void Draw(SpriteBatch spriteBatch) {
            foreach (Particle sprite in Shots)
                sprite.Draw(spriteBatch);
        }




        #region Effects Management methods
        /// <summary>
        /// Adds a shot to the list of particles
        /// </summary>
        /// <param name="location">The location of where the shot originates</param>
        /// <param name="velocity">The vector of the shot where it's going</param>
        /// <param name="frame">The frame in the spritesheet for the particle</param>
        private static void AddShot(Vector2 location, Vector2 velocity, int frame) {
            Particle shot = new Particle(location, Texture, shotRectangle, velocity, Vector2.Zero, 400f, 120, Color.White, Color.White);
            shot.AddFrame(new Rectangle(shotRectangle.X + shotRectangle.Width, shotRectangle.Y, shotRectangle.Width, shotRectangle.Height));
            shot.Animate = false;
            shot.Frame = frame;
            shot.RotateTo(velocity);
            Shots.Add(shot);
        }
        #endregion


        #region Weapons Management Methods
        /// <summary>
        /// Adds a shot for the currently fired weapon
        /// </summary>
        /// <param name="location"></param>
        /// <param name="velocity"></param>
        public static void FireWeapon(Vector2 location, Vector2 velocity) {
            AddShot(location, velocity, 0);
            _shotTimer = 0.0f;
        }
        #endregion
    }
}
