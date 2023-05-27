using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using System.Windows.Controls;
using Gomoku.Logic.AI;

namespace Gomoku.Logic
{
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
                point = game.Players.FirstOrDefault(x => x.PlayerName == game.CurrentPlayer.PlayerName).Piece.TypeIndex == piece.TypeIndex ? double.MaxValue : double.MinValue;
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
                    var tiles = StaticMethods.GetBlockedAndSameTiles(game.Board, new Tile(position.Item1, position.Item2), piece,
                        orientation, 5, 1);

                    var sameCounter = tiles.Item1.Count();
                    var blockedCounter = tiles.Item2.Count();

                    switch (blockedCounter)
                    {
                        case 0 when sameCounter + 1 >= 5:
                            point += sameCounter;
                            break;
                        case 0:
                            point += (1 - Math.Pow(2, sameCounter)) / -1;
                            break;
                        case 1:
                            point += sameCounter;
                            break;
                    }
                }
            }
            return point;
        }
    }
}
