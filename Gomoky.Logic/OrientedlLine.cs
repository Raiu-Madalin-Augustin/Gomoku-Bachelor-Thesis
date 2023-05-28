using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Gomoku.Logic
{
    public class OrientedlLine
    {
        private OrientedlLine(
          Piece piece,
          Orientations orientation,
          DirectionalLine firstLine,
          DirectionalLine secondLine)
        {
            Piece = piece;
            Orientation = orientation;
            FirstLine = firstLine;
            SecondLine = secondLine;

            Lines = ImmutableArray.Create(firstLine, secondLine);
            BlankTilesCount = firstLine.BlankTiles.Length + secondLine.BlankTiles.Length;
            BlockTilesCount = firstLine.BlockTiles.Length + secondLine.BlockTiles.Length;
            SameTileCount = firstLine.SameTiles.Length + secondLine.SameTiles.Length;
            TilesCount = firstLine.Tiles.Length + secondLine.Tiles.Length;
            IsChained = firstLine.IsChained && secondLine.IsChained;
        }

        public int BlankTilesCount { get; }

        public int BlockTilesCount { get; }

        public DirectionalLine FirstLine { get; }

        public bool IsChained { get; }

        public ImmutableArray<DirectionalLine> Lines { get; }

        public Orientations Orientation { get; }

        public Piece Piece { get; }

        public int SameTileCount { get; }

        public DirectionalLine SecondLine { get; }

        public int TilesCount { get; }

        public static OrientedlLine FromBoard(
          Board board,
          int x,
          int y,
          Piece type,
          Orientations orientation,
          int maxTile,
          int blankTolerance)
        {
            return orientation switch
            {
                Orientations.Horizontal => new OrientedlLine(type, orientation,
                    DirectionalLine.FromBoard(board, x, y, type, Directions.Left, maxTile, blankTolerance),
                    DirectionalLine.FromBoard(board, x, y, type, Directions.Right, maxTile, blankTolerance)),
                Orientations.Vertical => new OrientedlLine(type, orientation,
                    DirectionalLine.FromBoard(board, x, y, type, Directions.Up, maxTile, blankTolerance),
                    DirectionalLine.FromBoard(board, x, y, type, Directions.Down, maxTile, blankTolerance)),
                Orientations.Diagonal => new OrientedlLine(type, orientation,
                    DirectionalLine.FromBoard(board, x, y, type, Directions.UpLeft, maxTile, blankTolerance),
                    DirectionalLine.FromBoard(board, x, y, type, Directions.DownRight, maxTile, blankTolerance)),
                Orientations.SecondDiagonal => new OrientedlLine(type, orientation,
                    DirectionalLine.FromBoard(board, x, y, type, Directions.UpRight, maxTile, blankTolerance),
                    DirectionalLine.FromBoard(board, x, y, type, Directions.DownLeft, maxTile, blankTolerance)),
                _ => throw new InvalidOperationException("Unexpected value")
            };
        }

        public IEnumerable<Tile> GetSameTiles()
        {
            return Lines.SelectMany(l => l.SameTiles);
        }
    }
}