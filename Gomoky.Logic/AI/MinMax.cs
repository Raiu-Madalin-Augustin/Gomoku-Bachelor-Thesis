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

        public MinMax(Game gameCopy)
        {
            _gameCopy = gameCopy;
        }

        public Tuple<int, int> Analyze()
        {
            if (_gameCopy.IsOver)
                return Tuple.Create(-1, -1);

            var possibleMoves = GetPossibleMoves(_gameCopy.Board);

            var max = double.MinValue;

            foreach (var possibleMove in possibleMoves)
            {
                _gameCopy.Play(possibleMove[0], possibleMove[1]);

            }

            return Tuple.Create(0, 0);
        }


        public List<int[]> GetPossibleMoves(Board board)
        {
            var possibleMoves = new List<int[]>();
            var rows = board.Height;
            var cols = board.Width;

            // Iterate through each cell on the board and check if it's empty
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    if (board[i, j].Piece == Pieces.None)
                    {
                        possibleMoves.Add(new int[] { i, j });
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
                    var tile = game.Board[position[0], position[1]];
                    game.Play(tile.X, tile.Y);
                    var child = MinMaxEvaluate(game, new Tuple<int, int>(tile.X, tile.Y), player, depth - 1, alpha,
                        beta, false);
                }

            }
            return 0;

        }

        private double EvaluateGame(Game game, Tuple<int, int> move, Player player)
        {
            throw new NotImplementedException();
        }
    }
}