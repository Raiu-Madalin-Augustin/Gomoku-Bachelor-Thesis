using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku.Logic
{
    public class Player
    {
        public Piece Piece { get; set; }

        public IGomokuBase? GomokuAi { get; set; }

        public bool AutoPlay { get; set; }

        public string PlayerName { get; set; }

        public Player(string name, Piece piece, IGomokuBase gomoku, bool auto = false)
        {
            PlayerName = name;
            Piece = piece;
            GomokuAi = gomoku;
            AutoPlay = auto;
        }

        public Player(string name, Piece piece, bool auto = false)
        {
            PlayerName = name;
            Piece = piece;
            AutoPlay = auto;
        }
    }
}
