using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoky.Logic
{
    public class Board
    {
        private readonly Tile[,] _tiles;

        public int Width { get; }
        public int Height { get; }
        public Tile this[int x, int y] => _tiles[x, y];

        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            _tiles = new Tile[Width, Height];

            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    _tiles[i, j] = new Tile(i, j);
                }
            }
        }

    }
}
