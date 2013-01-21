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

            GenerateRandomMap();
        }


        /// <summary>
        /// Draw!
        /// </summary>
        /// <param name="spriteBatch"></param>
        static public void Draw(SpriteBatch spriteBatch) {
            int startX = GetSquareByPixelX((int)Camera.Position.X);
            int endX = GetSquareByPixelY((int)Camera.Position.X + Camera.ViewPortWidth);

            int startY = GetSquareByPixelY((int)Camera.Position.Y);
            int endY = GetSquareByPixelY((int)Camera.Position.Y + Camera.ViewPortHeight);

            for(int x  = startX; x <= endX; x++)
                for (int y = startY; y <= endY; y++) {
                    if (isWithinBounds(x, y))
                        spriteBatch.Draw(_texture, SquareScreenRectangle(x, y), _tiles[GetTileAtSquare(x, y)], Color.White);
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
                (squareY * TileHeight) + (TileHeight / 2));
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




        static public Rectangle SquareWorldRectangle(int x, int y) {
            return new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight);
        }



        static public Rectangle SquareWorldRectangle(Vector2 square) {
            return SquareWorldRectangle((int)square.X, (int)square.Y);
        }



        static public Rectangle SquareScreenRectangle(int x, int y) {
            return Camera.Transform(SquareWorldRectangle(x, y));
        }



        static public Rectangle SquareScreenRectangle(Vector2 square) {
            return SquareScreenRectangle((int)square.X, (int)square.Y);
        }
        #endregion



        #region Map Tile Information
        /// <summary>
        /// Gets the tile at the specified location in the square
        /// </summary>
        /// <param name="tileX">The X value of the coordinate</param>
        /// <param name="tileY">The Y value of the coordinate</param>
        /// <returns>The tile number, or -1</returns>
        static public int GetTileAtSquare(int tileX, int tileY) {
            if (isWithinBounds(tileX, tileY))
                return _mapSquares[tileX, tileY];
            else
                return -1;
        }



        /// <summary>
        /// Sets the specified tile at the specified location
        /// </summary>
        /// <param name="tileX">The X value of the coordinate</param>
        /// <param name="tileY">The Y value of the coordinate</param>
        /// <param name="tile">The value of tile to set</param>
        static public void SetTileAtSquare(int tileX, int tileY, int tile) {
            if (isWithinBounds(tileX, tileY))
                _mapSquares[tileX, tileY] = tile;
        }


        /// <summary>
        /// Checks whether the specified coordinate values are within the bounds of the map
        /// </summary>
        /// <param name="tileX">The x coordinate value</param>
        /// <param name="tileY">The y coordinate value</param>
        /// <returns>True if within bounds, otherwise, false</returns>
        static private bool isWithinBounds(int tileX, int tileY) {
            if ((tileX >= 0) && (tileX < MapWidth) && (tileY >= 0) && (tileY < MapHeight))
                return true;
            else
                return false;
        }



        /// <summary>
        /// Gets the tile at the specified pixel
        /// </summary>
        /// <param name="pixelX">The x val of the location</param>
        /// <param name="pixelY">The y val of the location</param>
        /// <returns></returns>
        static public int GetTileAtPixel(int pixelX, int pixelY) {
            return GetTileAtSquare(
                GetSquareByPixelX(pixelX),
                GetSquareByPixelY(pixelY));
        }
        #endregion


        #region Tile Handling
        /// <summary>
        /// Checks to see if the tile is a wall. This is done by comparing the tile index with the number where we say that 'from here on out, everything is a wall'
        /// </summary>
        /// <param name="tileX">The x location</param>
        /// <param name="tileY">The y location</param>
        /// <returns>True if it's a wall, false otherwise</returns>
        static public bool IsWallTile(int tileX, int tileY) {
            int tileIndex = GetTileAtSquare(tileX, tileY);
            if (tileIndex == -1)
                return false;
            else
                return tileIndex >= WallTileStart;
        }



        /// <summary>
        /// Checks to see if the tile is a wall
        /// </summary>
        /// <param name="square">The square vector value</param>
        /// <returns>True if it's a wall, false otherwise</returns>
        static public bool IsWallTile(Vector2 square) {
            return IsWallTile((int)square.X, (int)square.Y);
        }


        /// <summary>
        /// Checks to see if the tile is a wall
        /// </summary>
        /// <param name="pixelLocation">The location based on pixels</param>
        /// <returns>True if it's a wall, false otherwise</returns>
        static public bool IsWallTileByPixel(Vector2 pixelLocation) {
            return IsWallTile(
                GetSquareByPixelX((int)pixelLocation.X),
                GetSquareByPixelY((int)pixelLocation.Y)
                );
        }
        #endregion



        #region Map Generation
        static public void GenerateRandomMap() {
            int wallChancePerSquare = 10;
            int floorTile = rand.Next(FloorTileStart, FloorTileEnd + 1);
            int wallTile = rand.Next(WallTileStart, WallTileEnd + 1);
            for(int x = 0; x<MapWidth; x++)
                for (int y = 0; y < MapHeight; y++) {
                    _mapSquares[x, y] = floorTile;

                    if ((x == 0) || (y == 0) || (x == MapWidth - 1) || (y == MapHeight - 1)) {
                        _mapSquares[x, y] = wallTile;
                        continue;
                    }

                    if ((x == 1) || (y == 1) || (x == MapWidth - 2) || (y == MapHeight - 2)) {
                        //_mapSquares[x, y] = wallTile;
                        continue;
                    }

                    if (rand.Next(0, 100) <= wallChancePerSquare)
                        _mapSquares[x, y] = wallTile;
                }
        }
        #endregion
    } // end class
} // end namespace
