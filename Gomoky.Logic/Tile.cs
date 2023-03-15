using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoky.Logic
{
    public class Tile
    {
        public int X { get; }
        public int Y { get; }

        public Tile(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
