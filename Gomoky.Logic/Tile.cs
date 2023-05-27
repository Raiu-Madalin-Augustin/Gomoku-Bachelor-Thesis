using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku.Logic
{
    public class Tile
    {
        public int X { get; }
        public int Y { get; }
        public Piece Piece { get; set; }
        public Tile(int x, int y, Piece piece)
        {
            X = x;
            Y = y;
            Piece = piece;
        }
        public Tile(int x, int y)
        {
            X = x;
            Y = y;
            Piece = new Piece();
        }
    }
}
