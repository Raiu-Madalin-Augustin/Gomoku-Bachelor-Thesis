using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku.Logic.AI
{
    public enum Orientations
    {
        None = 0,
        Horizontal = 1,
        Vertical = 2,
        Diagonal = 3,
        SecondDiagonal = 4
    }

    public enum Directions
    {
        UpRight = 0,
        UpLeft = 1,
        DownRight = 2,
        DownLeft = 3,
        Left = 4,
        Right = 5,
        Up = 6,
        Down = 7
    }
    public class MinMax : IGomokuBase
    {
        private IEvaluator _evaluator { get; set; }

        public int Level { get; set; }
        public MinMax()
        {
            Level = 1;
            _evaluator = new Evaluator();
        }

        public Tuple<int, int> Analyze(Game game)
        {
            if (game.IsOver)
                return Tuple.Create(-1, -1);

            if (game.MoveHistory.Count == 0)
            {
                var tile = game.Board[game.Board.Width / 2, game.Board.Height / 2];
                return Tuple.Create(tile.X, tile.Y);
            }

            var forPlayer = game.CurrentPlayer;
            var possibleMoves = GetPossibleMoves(game.Board);

            var max = double.MinValue;
            var evaluations = new List<(Tuple<int, int>, double value)>();

            foreach (var possibleMove in possibleMoves)
            {
                game.Play(possibleMove.Item1, possibleMove.Item2);

                var value = MinMaxEvaluate(game, possibleMove, forPlayer, Level, isMaximizing: false);

                //if it reached max val , no need for further evaluations
                if (value == double.MaxValue)
                {
                    return possibleMove;
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
                game.Undo();
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
                    if (board[i, j].Piece.TypeIndex == 0)
                    {
                        possibleMoves.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            return possibleMoves;
        }

        public List<Tuple<int, int>> GetPossibleMoves(Game game, int maxDistance = 2, int tolerance = 1)
        {
            var unnavailableTiles = game.MoveHistory;

            var potentialTiles = new HashSet<Tuple<int, int>>();

            foreach (var tile in unnavailableTiles)
            {
                var orientations = new[]
                {
                    Orientations.Diagonal,
                    Orientations.Horizontal,
                    Orientations.SecondDiagonal,
                    Orientations.Vertical
                };

                foreach (var orientation in orientations)
                {
                    foreach (var t in GetMovesInDirection(game.Board, orientation, tile, maxDistance: maxDistance, tolerance))
                    {
                        potentialTiles.Add(Tuple.Create(t.X, t.Y));
                    }
                }
            }

            return potentialTiles.ToList();
        }

        private IEnumerable<Tile> GetMovesInDirection(Board board, Orientations orientation, Tile tile, int maxDistance, int tolerance)
        {
            var sameTiles = new Queue<Tile>();

            return orientation switch
            {
                Orientations.Horizontal => CombineLines(
                    GetLine(board, tile, Pieces.None, Directions.Left, maxDistance, tolerance),
                    GetLine(board, tile, Pieces.None, Directions.Right, maxDistance, tolerance)),
                Orientations.Diagonal => CombineLines(
                    GetLine(board, tile, Pieces.None, Directions.UpLeft, maxDistance, tolerance),
                    GetLine(board, tile, Pieces.None, Directions.DownRight, maxDistance, tolerance)
                    ),
                Orientations.Vertical => CombineLines(
                    GetLine(board, tile, Pieces.None, Directions.Up, maxDistance, tolerance),
                    GetLine(board, tile, Pieces.None, Directions.Down, maxDistance, tolerance)
                    ),
                Orientations.SecondDiagonal => CombineLines(
                    GetLine(board, tile, Pieces.None, Directions.UpRight, maxDistance, tolerance),
                    GetLine(board, tile, Pieces.None, Directions.DownLeft, maxDistance, tolerance)
                    )
            };

        }

        private static IEnumerable<Tile> GetLine(Board board, Tile tile, Pieces none, Directions upRight, int maxDistance, int tolerance)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Tile> CombineLines(IEnumerable<Tile> firstLine, IEnumerable<Tile> secondLine)
        {
            return firstLine.ToList().Concat(secondLine.ToList());
        }


        public double MinMaxEvaluate(Game game, Tuple<int, int> move, Player player, int depth,
            double alpha = double.MinValue, double beta = double.MaxValue, bool isMaximizing = true)
        {
            if (depth == 0)
            {
                return EvaluateGame(game, move, player, game.Players.First(x => x.Piece.TypeIndex != game.CurrentPlayer.Piece.TypeIndex));
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

                    game.Undo();

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

                    game.Undo();

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

            value += EvaluateGame(game, move, player, game.Players.FirstOrDefault(x => x.Piece.TypeIndex != game.CurrentPlayer.Piece.TypeIndex));
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