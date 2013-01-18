using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RobotRampage {
    static class TileMap {

        #region Declarations
        public const int TileWidth = 32;
        public const int TileHeight = 32;
        public const int MapWidth = 50;
        public const int MapHeight = 50;
        public const int FloorTileStart = 0;
        public const int FloorTileEnd = 3;
        public const int WallTileStart = 3;
        public const int WallTileEnd = 7;
        static private Texture2D _texture;
        static private List<Rectangle> _tiles = new List<Rectangle>();
        static private int[,] _mapSquares = new int[MapWidth, MapHeight];
        static private Random rand = new Random();
        #endregion


        /// <summary>
        /// Initializes the tile map
        /// </summary>
        /// <param name="tileTexture"></param>
        static public void Initialize(Texture2D tileTexture) {
            _texture = tileTexture;
            _tiles.Clear();
            _tiles.Add(new Rectangle(0, 0, TileWidth, TileHeight));
            _tiles.Add(new Rectangle(32, 0, TileWidth, TileHeight));
            _tiles.Add(new Rectangle(64, 0, TileWidth, TileHeight));
            _tiles.Add(new Rectangle(96, 0, TileWidth, TileHeight));
            _tiles.Add(new Rectangle(0, 32, TileWidth, TileHeight));
            _tiles.Add(new Rectangle(32, 32, TileWidth, TileHeight));
            _tiles.Add(new Rectangle(64, 32, TileWidth, TileHeight));
            _tiles.Add(new Rectangle(96, 32, TileWidth, TileHeight));

            for(int x = 0; x < MapWidth; x++)
                for (int y = 0; y < MapHeight; y++) {
                    _mapSquares[x, y] = FloorTileStart;
                }
        }


        #region Map Square Information
        /// <summary>
        /// Returns the square on the specified pixel on the X axis
        /// </summary>
        /// <param name="pixelX">The pixel where the square resides</param>
        /// <returns>The index of the square</returns>
        static public int GetSquareByPixelX(int pixelX) {
            return pixelX / TileWidth;
        }


        /// <summary>
        /// Returns the square on the specified pixel on the Y axis
        /// </summary>
        /// <param name="pixelY">The Y location of the pixel</param>
        /// <returns>The index of the square</returns>
        static public int GetSquareByPixelY(int pixelY) {
            return pixelY / TileHeight;
        }



        /// <summary>
        /// Returns the square at the specified pixel location
        /// </summary>
        /// <param name="pixelLocation">The coordinate of the square you want to get</param>
        /// <returns></returns>
        static public Vector2 GetSquareAtPixel(Vector2 pixelLocation) {
            return new Vector2(GetSquareByPixelX((int)pixelLocation.X), GetSquareByPixelY((int)pixelLocation.Y));
        }


        /// <summary>
        /// Returns the center of the square
        /// </summary>
        /// <param name="squareX">The x index value for the desired square</param>
        /// <param name="squareY">The y index value for the desired square</param>
        /// <returns></returns>
        public static Vector2 GetSquareCenter(int squareX, int squareY) {
            return new Vector2(
                (squareX * TileWidth) + (TileWidth / 2),
                (squareY * TileHeight) + (TileHeight / 2)));
        }



        /// <summary>
        /// Returns the center of the square
        /// 
        /// </summary>
        /// <param name="square"></param>
        /// <returns></returns>
        static public Vector2 GetSquareCenter(Vector2 square){
            return GetSquareCenter((int) square.X, (int)square.Y);
        }




        static public Rectangle SquareWorldRectangle(int x, int y)
        #endregion
    } // end class
} // end namespace
