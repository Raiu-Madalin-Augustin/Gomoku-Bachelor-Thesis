using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku.Logic.AI
{
    public class MinMax : IGomokuBase
    {
        private readonly Game _gameCopy;
        public int Level { get; set; }

        public MinMax(Game gameCopy, int level)
        {
            _gameCopy = gameCopy;
            Level = level;
        }

        public Tuple<int, int> Analyze()
        {
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

            return Tuple.Create(0, 0);
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
                return EvaluateGame(game, move, player);
            }

            if (game.IsOver)
            {
                return EvaluateGame(game, move, player);
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
            return 0;

        }

        private double EvaluateGame(Game game, Tuple<int, int> move, Player player)
        {
            throw new NotImplementedException();
        }
    }
}