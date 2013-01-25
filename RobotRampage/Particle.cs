using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RobotRampage {
    class Particle : Sprite {
        #region Declarations
        private Vector2 _acceleration;
        private float _maxSpeed;
        private int _initialDuration;
        private int _remainingDuration;
        private Color _initialColor;
        private Color _finalColor;
        #endregion


        public Particle(Vector2 location, Texture2D texture, Rectangle initialFrame, Vector2 velocity, Vector2 acceleration, float maxSpeed, int duration, Color initialColor, Color finalColor) :
            base(location, texture, initialFrame, velocity) {

                _initialDuration = duration;
                _remainingDuration = duration;
                _acceleration = acceleration;
                _initialColor = initialColor;
                _maxSpeed = maxSpeed;
                _finalColor = finalColor;
        }



        /// <summary>
        /// Update!
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            if (_remainingDuration <= 0) {
                Expired = true;
            }

            if (!Expired) {
                Velocity += _acceleration;
                if (Velocity.Length() > _maxSpeed) {
                    Vector2 vel = Velocity;
                    vel.Normalize();
                    Velocity = vel * _maxSpeed;
                }
                TintColor = Color.Lerp(_initialColor, _finalColor, DurationProgress);
                _remainingDuration--;
            }
            base.Update(gameTime);
        }



        public override void Draw(SpriteBatch spriteBatch) {
            if (IsActive) {
                base.Draw(spriteBatch);
            }
        }


        #region Properties
        public int ElapsedDuration {
            get { return _initialDuration - _remainingDuration; }
        }


        public float DurationProgress {
            get { return (float)ElapsedDuration / (float)_initialDuration; }
        }


        public bool IsActive {
            get { return (_remainingDuration > 0); }
        }
        #endregion
    }
}
