using System.Collections.Generic;

namespace Gomoku.Logic
{
    public class LineBasedCandidates : ICandidateSearcher
    {
        public IEnumerable<IPositional> Search(Game game, int maxTile = 2, int blankTolerance = 1)
        {
            var placedTiles = game.History;

            var tiles = new HashSet<Tile>();
            foreach (var tile in placedTiles)
            {
                var orientations = new[]
                {
                  Orientations.Horizontal,
                  Orientations.Vertical,
                  Orientations.Diagonal,
                  Orientations.SecondDiagonal
                };

                foreach (var orientation in orientations)
                {
                    foreach (var t in
                      OrientedlLine.FromBoard(
                        game.Board,
                        tile.X,
                        tile.Y,
                        (Piece)Pieces.None,
                        orientation,
                        maxTile: maxTile,
                        blankTolerance: blankTolerance)
                      .GetSameTiles())
                    {
                        tiles.Add(t);
                    }
                }
            }
            return tiles;
        }
    }
}