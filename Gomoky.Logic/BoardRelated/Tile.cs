﻿using System;
using System.Collections.Generic;

namespace Gomoku.Logic.BoardRelated
{
    public class Tile : ICoordinates, IDeepCloneable<Tile>, IShallowCloneable<Tile>
    {
        public Tile(int x, int y) : this(x, y, (Piece)Pieces.None)
        {
        }

        public Tile(int x, int y, Piece piece)
        {
            X = x;
            Y = y;
            Piece = piece;
        }

        public Piece Piece { get; set; }

        public int X { get; }

        public int Y { get; }

        public static bool operator !=(Tile t1, Tile t2)
        {
            return !(t1 == t2);
        }

        public static bool operator ==(Tile t1, Tile t2)
        {
            if (t1 is null && t2 is null)
            {
                return true;
            }

            if (t1 is null || t2 is null)
            {
                return false;
            }

            return t1.X == t2.X
              && t1.Y == t2.Y
              && t1.Piece == t2.Piece;
        }

        public Tile DeepClone()
        {
            return new Tile(X, Y, Piece);
        }

        public override bool Equals(object? obj)
        {
            return obj is Tile tile
              && EqualityComparer<Piece>.Default.Equals(Piece, tile.Piece)
              && X == tile.X
              && Y == tile.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Piece, X, Y);
        }

        public override string ToString()
        {
            return $"({X},{Y})={Piece}";
        }

        object IDeepCloneable.DeepClone()
        {
            return DeepClone();
        }
        object IShallowCloneable.ShallowClone()
        {
            return ShallowClone();
        }
        public Tile ShallowClone()
        {
            return (Tile)MemberwiseClone();
        }
    }
}