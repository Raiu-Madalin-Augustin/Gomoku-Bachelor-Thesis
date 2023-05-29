using System;
using System.Collections.Generic;
using System.Linq;
using Gomoku.Logic.BoardRelated;
using Gomoku.Logic.Evaluator;
using Gomoku.Logic.Lines;
using Gomoku.Logic.PlayerRelated;

namespace Gomoku.Logic.AI
{
    public class MiniMax : MiniMaxBase
    {
        public MiniMax(int level = 1)
        {
            Level = level;
            TileEvaluator = new Evaluate();
            CandidateSearcher = new LineBasedCandidates();
        }

        public int Level { get; set; }

        private ICandidateSearcher CandidateSearcher { get; set; }
        private ITileEvaluator TileEvaluator { get; set; }

        protected override AnalysisResult DoAnalyze(Game clonedGame)
        {
            if (clonedGame.IsOver)
            {
                return null;
            }

            var placedTiles = clonedGame.History;

            if (placedTiles.Count == 0)
            {
                return new AnalysisResult(
                  clonedGame.Board[clonedGame.Board.Width / 2, clonedGame.Board.Height / 2]);
            }

            var forPlayer = clonedGame.Manager.CurrentPlayer;

            var possiblePositions = CandidateSearcher.Search(clonedGame).ToList();

            var evaluations = new List<(ICoordinates positional, double value)>();

            var max = double.MinValue;

            foreach (var position in possiblePositions)
            {
                clonedGame.Play(position);

                var value = MiniMaxEvaluate(clonedGame, position, forPlayer, Level, isMaximizing: false);

                if (value == double.MaxValue)
                {
                    return new AnalysisResult(position);
                }

                if (value > max)
                {
                    evaluations.Clear();
                    evaluations.Add((position, value));
                    max = value;
                }
                else if (value == max)
                {
                    evaluations.Add((position, value));
                }

                clonedGame.Undo();
            }

            var choices =
              (from evaluation in evaluations
               select evaluation.positional)
               .ToList();

            var choice = Random.Next(choices.Count);
            return new AnalysisResult(choices[choice], choices);
        }

        private double EvaluateGame(Game game, ICoordinates positional, Player forPlayer, Player againstPlayer)
        {
            var value = TileEvaluator.EvaluatePosition(game, positional, againstPlayer.Piece);

            if (forPlayer != againstPlayer)
            {
                value = -value;
            }

            return value;
        }

        private double MiniMaxEvaluate(
          Game game,
          ICoordinates positional,
          Player forPlayer,
          int depth,
          double alpha = double.MinValue,
          double beta = double.MaxValue,
          bool isMaximizing = true)
        {
            if (depth == 0)
            {
                return EvaluateGame(game, positional, forPlayer, game.Manager.PreviousPlayer);
            }

            if (game.IsOver)
            {
                return EvaluateGame(game, positional, forPlayer, game.Manager.CurrentPlayer);
            }

            var playableTiles = CandidateSearcher.Search(game);

            double value;

            if (isMaximizing)
            {
                var maxValue = double.MinValue;

                foreach (var positional1 in playableTiles)
                {
                    var childTile = (Tile)positional1;
                    game.Play(childTile);

                    var childValue = MiniMaxEvaluate(game, childTile, forPlayer, depth - 1, alpha, beta, false);

                    game.Undo();

                    if (childValue == double.MinValue)
                    {
                        maxValue = childValue;
                        break;
                    }

                    maxValue = Math.Max(maxValue, childValue);
                    alpha = Math.Max(alpha, childValue);
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

                foreach (var positional1 in playableTiles)
                {
                    var childTile = (Tile)positional1;
                    game.Play(childTile);

                    var childValue = MiniMaxEvaluate(game, childTile, forPlayer, depth - 1, alpha, beta, true);

                    game.Undo();

                    if (childValue == double.MaxValue)
                    {
                        minValue = childValue;
                        break;
                    }

                    minValue = Math.Min(minValue, childValue);
                    beta = Math.Min(beta, childValue);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                value = minValue;
            }

            value += EvaluateGame(game, positional, forPlayer, game.Manager.PreviousPlayer);
            return value;
        }
    }
}