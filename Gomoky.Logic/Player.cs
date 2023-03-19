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

        public GomokuBase gomokuAI { get; set; }

        public bool autoPlay { get; set; }

        public string playerName { get; set; }

        public Player(string name, Piece piece, GomokuBase gomoku, bool auto = false)
        {
            playerName = name;
            Piece = piece;
            gomokuAI = gomoku;
            autoPlay = auto;
        }

    }
}
