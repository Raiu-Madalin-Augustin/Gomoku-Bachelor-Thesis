using System.Collections.Generic;

namespace Gomoku.Logic
{
  public interface ICandidateSearcher
  {
    public IEnumerable<IPositional> Search(Game game, int maxDistance = 2, int tolerance = 1);
  }
}