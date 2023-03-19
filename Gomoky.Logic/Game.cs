using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku.Logic
{
    public class Game
    {
        public Board Board { get; set; }
        public bool isOver { get; set; }

        public Game(int width, int height, IEnumerable<Player> players)
        {
            Board = new Board(width, height);
            isOver = false;
        }

    }
}
