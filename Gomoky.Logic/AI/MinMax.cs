using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku.Logic.AI
{
    public class MinMax : IGomokuBase
    {
        private Game _gameCopy;
        private IEvaluator _evaluator { get; set; }

        public int Level { get; set; }
        public MinMax()
        {
            Level = 1;
            _evaluator = new Evaluator();
        }

        public Tuple<int, int> Analyze(Game game)
        {
            _gameCopy = game.DeepClone();
            if (_gameCopy.IsOver)
                return Tuple.Create(-1, -1);

            var possibleMoves = GetPossibleMoves(_gameCopy.Board);

            var max = double.MinValue;
            var evaluations = new List<(Tuple<int, int>, double value)>();

            foreach (var possibleMove in possibleMoves)
            {
                _gameCopy.Play(possibleMove.Item1, possibleMove.Item2);

                var value = MinMaxEvaluate(_gameCopy, new Tuple<int, int>(possibleMove.Item1, possibleMove.Item2),
                    _gameCopy.CurrentPlayer, Level, isMaximizing: false);

                //if it reached max val , no need for further evaluations
                if (value == double.MaxValue)
                {
                    return Tuple.Create(possibleMove.Item1, possibleMove.Item2);
                }

                if (value > max)
                {
                    evaluations.Clear();
                    evaluations.Add((possibleMove, value));
                    max = value;
                }
                else if (value == max)
                {
                    evaluations.Add((possibleMove, value));
                }

                //ToDo, undo

            }

            var choices =
                (from evaluation in evaluations
                 select evaluation.Item1)
                .ToList();

            // Randomly pick one result from the choices
            var choice = new Random().Next(choices.Count);
            return choices[choice];
        }


        public List<Tuple<int, int>> GetPossibleMoves(Board board)
        {
            var possibleMoves = new List<Tuple<int, int>>();
            var rows = board.Height;
            var cols = board.Width;

            // Iterate through each cell on the board and check if it's empty
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    if (board[i, j].Piece == Pieces.None)
                    {
                        possibleMoves.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            return possibleMoves;
        }

        public double MinMaxEvaluate(Game game, Tuple<int, int> move, Player player, int depth,
            double alpha = double.MinValue, double beta = double.MaxValue, bool isMaximizing = true)
        {
            if (depth == 0)
            {
                return EvaluateGame(game, move, player, game.Players.FirstOrDefault(x => x.PlayerName != game.CurrentPlayer.PlayerName));
            }

            if (game.IsOver)
            {
                return EvaluateGame(game, move, player, game.CurrentPlayer);
            }

            var possibleMoves = GetPossibleMoves(game.Board);

            double value = 0;

            if (isMaximizing)
            {
                var maxValue = double.MinValue;

                foreach (var position in possibleMoves)
                {
                    var tile = game.Board[position.Item1, position.Item2];
                    game.Play(tile.X, tile.Y);
                    var child = MinMaxEvaluate(game, new Tuple<int, int>(tile.X, tile.Y), player, depth - 1, alpha,
                        beta, false);

                    //game.Undo();

                    if (child == double.MinValue)
                    {
                        maxValue = child;
                        break;
                    }

                    maxValue = Math.Max(maxValue, child);
                    alpha = Math.Max(alpha, child);

                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                value = maxValue;
            }
            else
            {
                var minValue = double.MaxValue;

                foreach (var position in possibleMoves)
                {
                    game.Play(position.Item1, position.Item2);

                    var child = MinMaxEvaluate(game, position, player, depth - 1, alpha, beta, true);

                    //game.Undo();

                    if (child == double.MaxValue)
                    {
                        minValue = child;
                        break;
                    }

                    minValue = Math.Min(minValue, child);
                    beta = Math.Min(beta, child);

                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                value = minValue;
            }

            value += EvaluateGame(game, move, player, game.Players.FirstOrDefault(x => x.PlayerName != game.CurrentPlayer.PlayerName));
            return value;
        }

        private double EvaluateGame(Game game, Tuple<int, int> move, Player player, Player versus)
        {
            var value = _evaluator.Evaluate(game, move, player.Piece, versus.Piece);

            if (player.Piece.TypeIndex != versus.Piece.TypeIndex)
            {
                value = -value;
            }

            return value;
        }
    }
}