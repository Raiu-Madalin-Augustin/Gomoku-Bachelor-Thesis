using System;
using System.Collections.Generic;
using System.Linq;

namespace Gomoku.Logic.AI
{
  public class AnalysisResult
  {
    public AnalysisResult(ICoordinates selectedChoice)
    {
      SelectedChoice = selectedChoice;
      PossibleChoices = new List<ICoordinates>()
      {
        selectedChoice
      };
    }

    public AnalysisResult(ICoordinates selectedChoice, IEnumerable<ICoordinates> possibleChoices)
    {
        PossibleChoices = possibleChoices ?? throw new ArgumentNullException(nameof(possibleChoices));
      SelectedChoice = selectedChoice;
    }

    public IEnumerable<ICoordinates> PossibleChoices { get; }
    public ICoordinates SelectedChoice { get; }
  }
}