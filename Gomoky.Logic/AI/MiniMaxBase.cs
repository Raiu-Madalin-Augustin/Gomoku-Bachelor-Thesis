using System;

namespace Gomoku.Logic.AI
{
  public abstract class MiniMaxBase
  {
    protected static readonly Random Random = new Random();

    public AnalysisResult Analyze(Game game)
    {
      var clonedGame = game.DeepClone();
      return DoAnalyze(clonedGame);
    }

    protected abstract AnalysisResult DoAnalyze(Game clonedGame);
  }
}