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
        private static Vector2 _baseAngle = Vector2.Zero;
        private static Vector2 _turretAngle = Vector2.Zero;
        private static float _playerSpeed = 190f;
        private static Rectangle _scrollArea = new Rectangle(150, 100, 500, 400);
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
            for (int x = 1; x < baseFrameCount; x++)
                BaseSprite.AddFrame(new Rectangle(baseInitialFrame.X + (frameHeight * x), baseInitialFrame.Y, frameWidth, frameHeight));

            TurretSprite = new Sprite(worldLocation, texture, turretInitialFrame, Vector2.Zero);
            // i think this should be turrentSprite, not basesprite
            for (int x = 1; x < turretFrameCount; x++)
                BaseSprite.AddFrame(new Rectangle(turretInitialFrame.X + (frameHeight * x), turretInitialFrame.Y, frameWidth, frameHeight));

        }



        /// <summary>
        /// Update!
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime) {
            HandleInput(gameTime);
            BaseSprite.Update(gameTime);
            ClamptToWorld();
            TurretSprite.WorldLocation = BaseSprite.WorldLocation;
        }



        /// <summary>
        /// Draw!
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void Draw(SpriteBatch spriteBatch) {
            BaseSprite.Draw(spriteBatch);
            TurretSprite.Draw(spriteBatch);
        }


        #region Input Handling
        /// <summary>
        /// Handles the keyboard movement issue
        /// </summary>
        /// <param name="keyState"></param>
        /// <returns></returns>
        private static Vector2 HandleKeyboardMovement(KeyboardState keyState) {
            Vector2 keyMovement = Vector2.Zero;
            if (keyState.IsKeyDown(Keys.W))
                keyMovement.Y--;
            if (keyState.IsKeyDown(Keys.A))
                keyMovement.X--;
            if (keyState.IsKeyDown(Keys.S))
                keyMovement.Y++;
            if (keyState.IsKeyDown(Keys.D))
                keyMovement.X++;

            return keyMovement;
        }


        /// <summary>
        /// Handles the gamepadState
        /// </summary>
        /// <param name="gamepadState">The gamepad state</param>
        private static Vector2 HandleGamepadMovement(GamePadState gamepadState) {
            Vector2 playerInput = new Vector2(gamepadState.ThumbSticks.Left.X, -gamepadState.ThumbSticks.Left.Y);
            return playerInput;
        }


        /// <summary>
        /// Handles shot controls from the user input from the keyboard
        /// </summary>
        /// <param name="keyboardState"></param>
        /// <returns></returns>
        private static Vector2 HandleKeyboardShots(KeyboardState keyState) {
            Vector2 keyShots = Vector2.Zero;
            if (keyState.IsKeyDown(Keys.Space))
                keyShots = new Vector2(-1, 1);
            if (keyState.IsKeyDown(Keys.NumPad2))
                keyShots = new Vector2(0, 1);
            if (keyState.IsKeyDown(Keys.NumPad3))
                keyShots = new Vector2(1, 1);
            if (keyState.IsKeyDown(Keys.NumPad4))
                keyShots = new Vector2(-1, 0);
            if (keyState.IsKeyDown(Keys.NumPad6))
                keyShots = new Vector2(1, 0);
            if (keyState.IsKeyDown(Keys.NumPad7))
                keyShots = new Vector2(-1, -1);
            if (keyState.IsKeyDown(Keys.NumPad8))
                keyShots = new Vector2(0, 1);
            if (keyState.IsKeyDown(Keys.NumPad9))
                keyShots = new Vector2(1, -1);

            return keyShots;
        }


        /// <summary>
        /// Handles the input from the gamepad for shots being fired
        /// </summary>
        /// <param name="gamepadState"></param>
        private static Vector2 HandleGamepadShots(GamePadState gamepadState) {
            return new Vector2(gamepadState.ThumbSticks.Right.X, -gamepadState.ThumbSticks.Right.Y);
        }


        private static void HandleInput(GameTime gameTime) {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 moveAngle = Vector2.Zero;
            Vector2 fireAngle = Vector2.Zero;
            moveAngle += HandleKeyboardMovement(Keyboard.GetState());
            moveAngle += HandleGamepadMovement(GamePad.GetState(PlayerIndex.One));

            fireAngle += HandleKeyboardShots(Keyboard.GetState());
            fireAngle += HandleGamepadShots(GamePad.GetState(PlayerIndex.One));

            if (moveAngle != Vector2.Zero) {
                moveAngle.Normalize();
                _baseAngle = moveAngle;
                moveAngle = CheckTileObstacles(elapsed, moveAngle);
            }

            if (fireAngle != Vector2.Zero) {
                fireAngle.Normalize();
                _turretAngle = fireAngle;
            }


            BaseSprite.RotateTo(_baseAngle);
            TurretSprite.RotateTo(_turretAngle);
            BaseSprite.Velocity = moveAngle * _playerSpeed;

            RepositionCamera(gameTime, moveAngle);
        }
        #endregion



        #region Movement Limitations
        /// <summary>
        /// Limits the user to the size of the world map
        /// </summary>
        private static void ClamptToWorld() {
            float currentX = BaseSprite.WorldLocation.X;
            float currentY = BaseSprite.WorldLocation.Y;
            currentX = MathHelper.Clamp(currentX, 0, Camera.WorldRectangle.Right - BaseSprite.FrameWidth);
            currentY = MathHelper.Clamp(currentY, 0, Camera.WorldRectangle.Bottom - BaseSprite.FrameHeight);
            BaseSprite.WorldLocation = new Vector2(currentX, currentY);
        }



        /// <summary>
        /// Repositions the game camera
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="moveAngle"></param>
        private static void RepositionCamera(GameTime gameTime, Vector2 moveAngle) {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float moveScale = _playerSpeed * elapsed;

            if ((BaseSprite.ScreenRectangle.X < _scrollArea.X) && (moveAngle.X < 0))
                Camera.Move(new Vector2(moveAngle.X, 0) * moveScale);

            if ((BaseSprite.ScreenRectangle.Right > _scrollArea.Right) && (moveAngle.X > 0))
                Camera.Move(new Vector2(moveAngle.X, 0) * moveScale);

            if ((BaseSprite.ScreenRectangle.Y < _scrollArea.Y) && (moveAngle.Y < 0))
                Camera.Move(new Vector2(0, moveAngle.Y) * moveScale);

            if ((BaseSprite.ScreenRectangle.Bottom > _scrollArea.Bottom) && (moveAngle.Y > 0))
                Camera.Move(new Vector2(0, moveAngle.Y) * moveScale);
        }



        private static Vector2 CheckTileObstacles(float elapsedTime, Vector2 moveAngle) {
            Vector2 newHorizontalLocation = BaseSprite.WorldLocation + (new Vector2(moveAngle.X, 0) * (_playerSpeed * elapsedTime));
            Vector2 newVerticalLocation = BaseSprite.WorldLocation + (new Vector2(0, moveAngle.Y) * (_playerSpeed * elapsedTime));

            Rectangle newHorizontalRect = new Rectangle((int)newHorizontalLocation.X, (int)BaseSprite.WorldLocation.Y, BaseSprite.FrameWidth, BaseSprite.FrameHeight);
            Rectangle newVerticalRect = new Rectangle((int)BaseSprite.WorldLocation.X, (int)newVerticalLocation.Y, BaseSprite.FrameWidth, BaseSprite.FrameHeight);

            int horizLeftPixel = 0;
            int horizRightPixel = 0;
            int vertTopPixel = 0;
            int vertBottomPixel = 0;

            if (moveAngle.X < 0) {
                horizLeftPixel = (int)newHorizontalRect.Left;
                horizRightPixel = (int)BaseSprite.WorldRectangle.Left;
            }


            if (moveAngle.X > 0) {
                horizLeftPixel = (int)BaseSprite.WorldRectangle.Right;
                horizRightPixel = (int)newHorizontalRect.Right;
            }

            if (moveAngle.Y < 0) {
                vertTopPixel = (int)newVerticalRect.Top;
                vertBottomPixel = (int)BaseSprite.WorldRectangle.Top;
            }

            if (moveAngle.Y > 0) {
                vertTopPixel = (int)BaseSprite.WorldRectangle.Bottom;
                vertBottomPixel = (int)newVerticalRect.Bottom;
            }

            if (moveAngle.X != 0) {
                for (int x = horizLeftPixel; x < horizRightPixel; x++) {
                    for (int y = 0; y < BaseSprite.FrameHeight; y++) {
                        if (TileMap.IsWallTileByPixel(new Vector2(x, newHorizontalLocation.Y + y))) {
                            moveAngle.X = 0;
                            break;
                        }
                    }
                    if (moveAngle.X == 0)
                        break;
                }
            }

            if (moveAngle.Y != 0) {
                for (int y = vertTopPixel; y < vertBottomPixel; y++) {
                    for (int x = 0; x < BaseSprite.FrameWidth; x++) {
                        if (TileMap.IsWallTileByPixel(new Vector2(newVerticalLocation.X + x, y))) {
                            moveAngle.Y = 0;
                            break;
                        }
                    }

                    if (moveAngle.Y == 0)
                        break;
                }
            }

            return moveAngle;
        }
        #endregion
    } // end class
} // end namespace
