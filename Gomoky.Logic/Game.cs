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

        /// <summary>
        /// Checks if the game is over.
        /// </summary>
        public bool IsOver { get; private set; }

        public Game(int width, int height, IEnumerable<Player> players)
        {
            Board = new Board(width, height);
            IsOver = false;
        }

        public void Play(int x, int y)
        {

        }
    }
}
