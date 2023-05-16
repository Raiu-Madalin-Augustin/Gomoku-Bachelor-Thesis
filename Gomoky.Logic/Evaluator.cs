using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using System.Windows.Controls;

namespace Gomoku.Logic
{
    public enum Direction
    {
        Horizontal,
        Vertical,
        Diagonal,
        ReverseDiagonal
    }

    public class Evaluator : IEvaluator
    {
        //evaluate game state
        //returns the score for the evaluation
        public double Evaluate(Game game, Tuple<int, int> position, Piece piece, Piece versus)
        {
            var point = 0.0;
            var x = position.Item1;
            var y = position.Item2;

            // If game is over the point should be high enough so that this node is
            // more likely to get noticed
            if (game.IsOver)
            {
                if (game.Players.FirstOrDefault(x => x.PlayerName == game.CurrentPlayer.PlayerName).Piece.TypeIndex == piece.TypeIndex)
                {
                    point = double.MaxValue;
                }
                else
                {
                    point = double.MinValue;
                }
            }
            else
            {
                // Evaluate horizontal line
                point += EvaluateLine(game, x, y, piece, 1, 0, versus);

                // Evaluate vertical line
                point += EvaluateLine(game, x, y, piece, 0, 1, versus);

                // Evaluate diagonal line
                point += EvaluateLine(game, x, y, piece, 1, 1, versus);

                // Evaluate reverse diagonal line
                point += EvaluateLine(game, x, y, piece, -1, 1, versus);
            }

            return point;
        }

        private double EvaluateLine(Game game, int x, int y, Piece piece, int dx, int dy, Piece versus)
        {
            var sameTilesCount = 0;
            var emptyTilesCount = 0;
            var blockTilesCount = 0;
            var currentX = x;
            var currentY = y;

            // Count same, empty, and block tiles in the line
            while (currentX >= 0 && currentX < 15 && currentY >= 0 && currentY < 15)
            {
                var tile = game.Board[currentX, currentY];

                if (tile.Piece.TypeIndex == piece.TypeIndex)
                {
                    sameTilesCount++;
                }
                else if (tile.Piece.TypeIndex == versus.TypeIndex)
                {
                    blockTilesCount++;
                    break;

                }
                else
                {
                    emptyTilesCount++;
                }


                currentX += dx;
                currentY += dy;
            }

            // Check the other side of the line
            currentX = x - dx;
            currentY = y - dy;

            while (currentX >= 0 && currentX < 15 && currentY >= 0 && currentY < 15)
            {
                var tile = game.Board[currentX, currentY];

                if (tile.Piece.TypeIndex == piece.TypeIndex)
                {
                    sameTilesCount++;
                }
                else if (tile.Piece == Pieces.None)
                {
                    emptyTilesCount++;
                }
                else
                {
                    blockTilesCount++;
                    break;
                }

                currentX -= dx;
                currentY -= dy;
            }

            // Calculate points based on same tiles, empty tiles, and block tiles
            var totalTiles = sameTilesCount + emptyTilesCount;
            var blockFactor = blockTilesCount == 1 ? 1.0 : 0.5;
            var sameFactor = sameTilesCount >= 15 ? 1000000.0 : Math.Pow(10, sameTilesCount);

            if (emptyTilesCount == 0)
            {
                return sameFactor * blockFactor;
            }
            else if (emptyTilesCount == 1)
            {
                return sameFactor * blockFactor * 0.5;
            }
            else if (emptyTilesCount == 2)
            {
                return sameFactor * blockFactor * 0.1;
            }
            else
            {
                return sameFactor * blockFactor * 0.01 * emptyTilesCount;
            }
        }
    }
}
