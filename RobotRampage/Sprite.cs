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



        public Sprite(Vector2 worldLocation, Texture2D texture, Rectangle initialFrame, Vector2 velocity) {
            _worldLocation = worldLocation;
            this.Texture = texture;
            _velocity = velocity;
            _frames.Add(initialFrame);
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
        #endregion
    }
}
