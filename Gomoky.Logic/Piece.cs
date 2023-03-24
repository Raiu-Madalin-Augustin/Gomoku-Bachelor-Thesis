using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku.Logic
{
    public readonly struct Piece
    {
        public Piece(int typeIndex)
        {
            TypeIndex = typeIndex;
        }

        public Piece(Pieces pieces) :
          this((int)pieces)
        {
        }

        public int TypeIndex { get; }

        public static implicit operator Pieces(Piece p)
        {
            return (Pieces)p.TypeIndex;
        }

    }
}
