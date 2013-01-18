using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RobotRampage {
    class Sprite {
        #region Declarations
        public Texture2D Texture;
        private Vector2 _worldLocation = Vector2.Zero;
        private Vector2 _velocity = Vector2.Zero;
        private List<Rectangle> _frames = new List<Rectangle>();
        private int _currentFrame;
        private float _frameTime = 0.1f;
        private float _timeForCurrentFrame = 0.0f;
        private Color _tintColor = Color.White;
        private float _rotation = 0.0f;
        public bool Expired = false;
        public bool Animate = true;
        public bool AnimateWhenStopped = true;
        public bool Collidable = true;
        public int CollisionRadius = 0;
        public int BoundingXPadding = 0;
        public int BoundingYPadding = 0;
        #endregion


        /// <summary>
        /// Creates a new sprite
        /// </summary>
        /// <param name="worldLocation">The location of the sprite in the world</param>
        /// <param name="texture">The spritesheet/texture to use</param>
        /// <param name="initialFrame">The rectangle of the location on texture to use as the first frame</param>
        /// <param name="velocity">The velocity of the sprite (if it's moving)</param>
        public Sprite(Vector2 worldLocation, Texture2D texture, Rectangle initialFrame, Vector2 velocity) {
            _worldLocation = worldLocation;
            this.Texture = texture;
            _velocity = velocity;
            _frames.Add(initialFrame);
        }


        /// <summary>
        /// Update!
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime) {
            if (!Expired) { //check that the sprite isnt expired
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                _timeForCurrentFrame += elapsed;

                // if it's an animated sprite...
                if (Animate) {
                    if (_timeForCurrentFrame >= FrameTime) {
                        if ((AnimateWhenStopped) || (_velocity != Vector2.Zero)) {
                            _currentFrame = (_currentFrame + 1) % (_frames.Count);
                            _timeForCurrentFrame = 0.0f;
                        }
                    }
                }

                // update the position of the sprite on the world map
                _worldLocation += (_velocity * elapsed);
            }
        }



        /// <summary>
        /// Draw!
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, ScreenCenter, Source, _tintColor, _rotation, RelativeCenter, 1.0f, SpriteEffects.None, 0.0f);
        }

        #region Drawing and Animation Properties
        public int FrameWidth {
            get { return _frames[0].Width; }
        }


        public int FrameHeight {
            get { return _frames[0].Height; }
        }


        public Color TintColor {
            get { return _tintColor; }
            set { _tintColor = value; }
        }


        public float Rotation {
            get { return _rotation; }
            set { _rotation = value % MathHelper.TwoPi; }
        }


        public int Frame {
            get { return _currentFrame; }
            set { _currentFrame = (int)MathHelper.Clamp(value, 0, _frames.Count - 1); }
        }


        public float FrameTime {
            get { return _frameTime; }
            set { _frameTime = MathHelper.Max(0, value); }
        }


        public Rectangle Source {
            get { return _frames[_currentFrame]; }
        }
        #endregion

        #region Positional Properties
        public Vector2 WorldLocation {
            get { return _worldLocation; }
            set { _worldLocation = value; }
        }


        public Vector2 ScreenLocation {
            get { return Camera.Transform(_worldLocation); }

        }


        public Vector2 Velocity {
            get { return _velocity; }
            set { _velocity = value; }
        }


        public Rectangle WorldRectangle {
            get {
                return new Rectangle(
                    (int)_worldLocation.X,
                    (int)_worldLocation.Y,
                    FrameWidth,
                    FrameHeight);
            }
        }


        public Rectangle ScreenRectangle {
            get { return Camera.Transform(WorldRectangle); }
        }


        public Vector2 RelativeCenter {
            get { return new Vector2(FrameWidth / 2, FrameHeight / 2); }
        }


        public Vector2 WorldCenter {
            get { return _worldLocation + RelativeCenter; }
        }


        public Vector2 ScreenCenter {
            get { return Camera.Transform(_worldLocation + RelativeCenter); }
        }
        #endregion

        #region Collision Related Properties
        public Rectangle BoundingBoxRect {
            get {
                return new Rectangle(
                      (int)_worldLocation.X + BoundingXPadding,
                      (int)_worldLocation.Y + BoundingYPadding,
                      FrameWidth - (BoundingXPadding * 2),
                      FrameHeight - (BoundingYPadding * 2));
            }
        }
        #endregion

        #region Collision Detection Methods
        /// <summary>
        /// Determines if the specified rectagle intersects this objects rectangle
        /// </summary>
        /// <param name="otherBox"></param>
        /// <returns></returns>
        public bool IsBoxColliding(Rectangle otherBox) {
            if (Collidable && !Expired) {
                return BoundingBoxRect.Intersects(otherBox);
            } else {
                return false;
            }
        }


        /// <summary>
        /// Checks whether the specified circle is intersecting with this circle
        /// </summary>
        /// <param name="otherCenter">The center of the circle</param>
        /// <param name="otherRadius">The radius of the circle</param>
        /// <returns></returns>
        public bool IsCircleColliding(Vector2 otherCenter, float otherRadius) {
            if ((Collidable) && (!Expired)) {
                if (Vector2.Distance(WorldCenter, otherCenter) < (CollisionRadius + otherRadius))
                    return true;
                else
                    return false;
            } else {
                return false;
            }
        }
        #endregion

        #region Animation-related Methods
        /// <summary>
        /// Adds a frame to the animation
        /// </summary>
        /// <param name="frameRectangle">The rectangle location of the sprite sheed to add to the animations</param>
        public void AddFrame(Rectangle frameRectangle) {
            _frames.Add(frameRectangle);
        }


        /// <summary>
        /// Rotates this sprite to the specified direction
        /// </summary>
        /// <param name="direction"></param>
        public void RotateTo(Vector2 direction) {
            Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }
        #endregion
    }
}
