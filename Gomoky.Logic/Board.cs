using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku.Logic
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
                    _tiles[i, j] = new Tile(i, j, new Piece(Pieces.None));
                }
            }
        }

        public Board(Board board)
        {
            Width = board.Width;
            Height = board.Height;
            _tiles = new Tile[Width, Height];
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    _tiles[i, j] = new Tile(i, j, board[i, j].Piece);
                }
            }
        }

        public Board DeepClone()
        {
            return new Board(this);
        }

        public bool IsValidPosition(Tuple<int,int> position)
        {
            return position.Item1 >= 0 && position.Item1 < Width && position.Item2 >= 0 && position.Item2 < Height;
        }
    }
}
