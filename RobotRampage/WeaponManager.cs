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
        static private float _rocketMinTimer = 0.5f;

        static public float WeaponTimeRemaining = 30.0f;
        static private float _weaponTimeDefault = 30.0f;
        static private float _tripleWeaponSplitAngle = 15;
        static public WeaponType CurrentWeaponType = WeaponType.Rocket;
        public enum WeaponType {
            Normal,
            Triple,
            Rocket
        };

        static public List<Sprite> PowerUps = new List<Sprite>();
        static private int _maxActivePowerups = 5;
        static private float _timeSinceLastPowerup = 0.0f;
        static private float _timeBetweenPowerups = 2.0f;
        static private Random rand = new Random();
        #endregion

        #region Properties
        static public float WeaponFireDelay {
            get {
                if (CurrentWeaponType == WeaponType.Rocket) {
                    return _rocketMinTimer;
                } else {
                    return _shotMinTimer;
                }
            }
        }

        static public bool CanFireWeapon {
            get { return (_shotTimer >= WeaponFireDelay); }
        }
        #endregion


        /// <summary>
        /// Update!
        /// </summary>
        /// <param name="gameTime"></param>
        static public void Update(GameTime gameTime) {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _shotTimer += elapsed;
            CheckWeaponUpgradeExpire(elapsed);

            for (int x = Shots.Count - 1; x >= 0; x--) {
                Shots[x].Update(gameTime);
                CheckShotWallImpacts(Shots[x]);

                if (Shots[x].Expired)
                    Shots.RemoveAt(x);
            }

            CheckPowerUpSpawns(elapsed);
            CheckPowerupPickups();
        }



        /// <summary>
        /// Draw!
        /// </summary>
        /// <param name="spriteBatch"></param>
        static public void Draw(SpriteBatch spriteBatch) {
            foreach (Particle sprite in Shots)
                sprite.Draw(spriteBatch);

            foreach (Sprite sprite in PowerUps)
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
            switch (CurrentWeaponType) {
                case WeaponType.Normal:
                    AddShot(location, velocity, 0);
                    break;

                case WeaponType.Triple:
                    AddShot(location, velocity, 0);
                    float baseAngle = (float)Math.Atan2(velocity.Y, velocity.X);
                    float offset = MathHelper.ToRadians(_tripleWeaponSplitAngle);
                    AddShot(location, new Vector2(
                            (float)Math.Cos(baseAngle - offset),
                            (float)Math.Sin(baseAngle - offset)) * velocity.Length(), 0);

                    AddShot(location, new Vector2(
                            (float)Math.Cos(baseAngle + offset),
                            (float)Math.Sin(baseAngle + offset)) * velocity.Length(), 0);
                    break;

                case WeaponType.Rocket:
                    AddShot(location, velocity, 1);
                    break;
            }

            _shotTimer = 0.0f;
        }



        /// <summary>
        /// Checks how much time is left on the current weapon and reduces the time
        /// </summary>
        /// <param name="elapsed"></param>
        private static void CheckWeaponUpgradeExpire(float elapsed) {
            if (CurrentWeaponType != WeaponType.Normal) {
                WeaponTimeRemaining -= elapsed;
            if (WeaponTimeRemaining <= 0) {
                CurrentWeaponType = WeaponType.Normal;
                WeaponTimeRemaining = 0.0f;
            }
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="type"></param>
        private static void TryToSpawnPowerup(int x, int y, WeaponType type) {
            if (PowerUps.Count >= _maxActivePowerups) {
                return;
            }

            Rectangle thisDestination = TileMap.SquareWorldRectangle(new Vector2(x, y));
            foreach(Sprite powerup in PowerUps){
                if (powerup.WorldRectangle == thisDestination)
                    return;
            }

            if (!TileMap.IsWallTile(x, y)) {
                Sprite newPowerUp = new Sprite(new Vector2(thisDestination.X, thisDestination.Y), Texture, new Rectangle(64, 128, 32, 32), Vector2.Zero);
                newPowerUp.Animate = false;
                newPowerUp.CollisionRadius = 14;
                newPowerUp.AddFrame(new Rectangle(96, 128, 32, 32));

                if (type == WeaponType.Rocket)
                    newPowerUp.Frame = 1;

                PowerUps.Add(newPowerUp);
                _timeSinceLastPowerup = 0.0f;
                
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="elapsed"></param>
        private static void CheckPowerUpSpawns(float elapsed) {
            _timeSinceLastPowerup += elapsed;
            if (_timeSinceLastPowerup >= _timeBetweenPowerups) {
                WeaponType type = WeaponType.Triple;
                if (rand.Next(0, 2) == 1) {
                    type = WeaponType.Rocket;
                }
                TryToSpawnPowerup(rand.Next(0, TileMap.MapWidth), rand.Next(0, TileMap.MapHeight), type);
            }
        }



        /// <summary>
        /// Check if a user has picked up a powerup
        /// </summary>
        private static void CheckPowerupPickups() {
            for (int x = PowerUps.Count - 1; x >= 0; x--) {
                if (Player.BaseSprite.IsCircleColliding(PowerUps[x].WorldCenter, PowerUps[x].CollisionRadius)) {
                    switch (PowerUps[x].Frame) {
                        case 0: CurrentWeaponType = WeaponType.Triple; break;
                        case 1: CurrentWeaponType = WeaponType.Rocket; break;
                    }
                    WeaponTimeRemaining = _weaponTimeDefault;
                    PowerUps.RemoveAt(x);
                }
            }
        }
        #endregion


        #region Collision Detection
        /// <summary>
        /// Check collision detection against walls
        /// </summary>
        /// <param name="shot"></param>
        private static void CheckShotWallImpacts(Sprite shot) {
            if (shot.Expired) 
                return;

            if (TileMap.IsWallTile(TileMap.GetSquareAtPixel(shot.WorldCenter))) {
                shot.Expired = true;
                if (shot.Frame == 0) {
                    EffectsManager.AddSparksEffect(shot.WorldCenter, shot.Velocity);
                } else {
                    CreateLargeExplosion(shot.WorldCenter);
                }
            }
        }


        /// <summary>
        /// Creates a rather large explosion effect
        /// </summary>
        /// <param name="location"></param>
        private static void CreateLargeExplosion(Vector2 location) {
            EffectsManager.AddLargeExplosion(location + new Vector2(-10, -10));
            EffectsManager.AddLargeExplosion(location + new Vector2(-10, 10));
            EffectsManager.AddLargeExplosion(location + new Vector2(10, -10));
            EffectsManager.AddLargeExplosion(location + new Vector2(-10, 10));
            EffectsManager.AddLargeExplosion(location);
        }
        #endregion
    }
}
