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
    } // end class
} // end namespace
