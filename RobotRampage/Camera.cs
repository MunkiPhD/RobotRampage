using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RobotRampage {
    public static class Camera {

        #region Declarations
        private static Vector2 _position = Vector2.Zero;
        private static Vector2 _viewPortSize = Vector2.Zero;
        private static Rectangle _worldRectangle = new Rectangle(0, 0, 0, 0);
        #endregion


        #region Properties
        public static Vector2 Position {
            get { return _position; }
            set { _position = new Vector2(MathHelper.Clamp(value.X, _worldRectangle.X, _worldRectangle.Width - ViewPortWidth), MathHelper.Clamp(value.Y, _worldRectangle.Y, _worldRectangle.Height - ViewPortHeight)); }
        }

        public static Rectangle WorldRectangle {
            get { return _worldRectangle; }
            set { _worldRectangle = value; }
        }

        public static int ViewPortWidth {
            get { return (int)_viewPortSize.X; }
            set { _viewPortSize.X = value; }
        }

        public static int ViewPortHeight {
            get { return (int)_viewPortSize.Y; }
            set { _viewPortSize.Y = value; }
        }

        public static Rectangle ViewPort {
            get {
                return new Rectangle((int)Position.X, (int)Position.Y, ViewPortWidth, ViewPortHeight);
            }
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Moves the camera with the specified offset
        /// </summary>
        /// <param name="offset">The vector to add to the current position vector</param>
        public static void Move(Vector2 offset) {
            Position += offset;
        }



        /// <summary>
        /// Tests whether the specified bounds are within the camera viewport
        /// </summary>
        /// <param name="bounds">The bounds of the object who's visibility to test</param>
        /// <returns></returns>
        public static bool ObjectIsVisible(Rectangle bounds) {
            return ViewPort.Intersects(bounds);
        }



        /// <summary>
        /// Calculates the position of the vector in relation to the viewport
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector2 Transform(Vector2 point) {
            return point - _position;
        }



        /// <summary>
        /// Calculates the position of the rectangle object in relation to the viewport
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static Rectangle Transform(Rectangle rectangle) {
            return new Rectangle(
                rectangle.Left - (int)_position.X,
                rectangle.Top - (int)_position.Y,
                rectangle.Width,
                rectangle.Height);
        }
        #endregion
    }
}
