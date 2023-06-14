using System;
using Gomoku.Logic.BoardRelated;
using Gomoku.Logic.Lines;

namespace Gomoku.Logic.Evaluator
{
    public class Evaluate : ITileEvaluator
    {
        public double EvaluatePosition(Game game, ICoordinates positional, Piece piece)
        {
            var point = 0.0;

            if (game.IsOver)
            {
                point = game.Manager.PreviousPlayer.Piece == piece ? double.MaxValue : double.MinValue;
            }
            else
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
                    var line =
                      OrientedlLine.FromBoard(
                        game.Board,
                        positional.X,
                        positional.Y,
                        piece,
                        orientation,
                        maxTile: Game.WinCondition,
                        blankTolerance: 1);

                    var sameTilesCount = line.SameTileCount;
                    var blockTilesCount = line.BlockTilesCount;

                    switch (blockTilesCount)
                    {
                        case 0 when sameTilesCount + 1 >= Game.WinCondition:
                            point += 1.0 * (1.0 - Math.Pow(2.0, sameTilesCount)) / (1.0 - 2.0) + 1;
                            break;
                        case 0:
                            {
                                // Calculate point using Geometric series of 2.0 so that the more
                                // chain it has, the more valuable the line
                                var pointValue =
                                    1.0 * (1.0 - Math.Pow(2.0, sameTilesCount)) / (1.0 - 2.0);

                                if (pointValue < 0)
                                {
                                    throw new Exception();
                                }

                                point += Math.Pow(pointValue, line.IsChained ? 2.0 : 1.5);
                                break;
                            }
                        case 1:
                            point += sameTilesCount;
                            break;
                    }
                }
            }

            return point;
        }
    }
}