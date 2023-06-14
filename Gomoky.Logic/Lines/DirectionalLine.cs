using System;
using System.Collections.Generic;
using System.Linq;
using Gomoku.Logic.BoardRelated;

namespace Gomoku.Logic.Lines
{
    public class DirectionalLine
    {
        private DirectionalLine(
          Piece piece,
          Directions direction,
          Tile[] tiles,
          Tile[] sameTiles,
          Tile[] blankTiles,
          Tile[] blockTiles,
          bool isChained)
        {
            Piece = piece;
            Direction = direction;
            Tiles = tiles;
            SameTiles = sameTiles;
            BlankTiles = blankTiles;
            BlockTiles = blockTiles;
            IsChained = isChained;
        }

        public Tile[] BlankTiles { get; }

        public Tile[] BlockTiles { get; }

        public Directions Direction { get; }

        public bool IsChained { get; }

        public Piece Piece { get; }

        public Tile[] SameTiles { get; }

        public Tile[] Tiles { get; }

        public static DirectionalLine FromBoard(
          Board board,
          int x,
          int y,
          Piece piece,
          Directions direction,
          int maxTile,
          int blankTolerance)
        {
            var tiles = new Queue<Tile>();
            var sameTiles = new Queue<Tile>();
            var blankTiles = new Queue<Tile>();
            var blockTiles = new Queue<Tile>();

            var count = 0;
            var chainBreak = false;
            var blank = 0;

            board.IterateTiles(
              x,
              y,
              direction,
              t =>
              {
                  if (count++ == maxTile)
                  {
                      return false;
                  }

                  if (t.Piece.Type == piece)
                  {
                      if (blank > 0)
                      {
                          chainBreak = true;
                      }

                      tiles.Enqueue(t);
                      sameTiles.Enqueue(t);
                      return true;
                  }
                  else if (t.Piece.Type == Pieces.None)
                  {
                      if (blank++ == blankTolerance)
                      {
                          return false;
                      }

                      tiles.Enqueue(t);
                      blankTiles.Enqueue(t);
                      return true;
                  }
                  else
                  {
                      tiles.Enqueue(t);
                      blockTiles.Enqueue(t);
                      return false;
                  }
              });

            return new DirectionalLine(
              piece,
              direction,
              tiles.ToArray(),
              sameTiles.ToArray(),
              blankTiles.ToArray(),
              blockTiles.ToArray(),
              !chainBreak);
        }
    }
}