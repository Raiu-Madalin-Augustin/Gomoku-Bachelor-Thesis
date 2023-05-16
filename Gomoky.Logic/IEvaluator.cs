using System;

namespace Gomoku.Logic;

public interface IEvaluator
{
    public double Evaluate(Game game, Tuple<int, int> position, Piece piece, Piece versus);
}