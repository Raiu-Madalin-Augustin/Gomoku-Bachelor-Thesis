using System;
using System.Collections.Generic;

namespace Gomoku.Logic
{
    public class GameOverEventArgs
    {
        public GameOverEventArgs(
          int turn,
          Player winner)
        {
            Turn = turn;
            Winner = winner;
        }

        public int Turn { get; }
        public Player Winner { get; }
    }
}