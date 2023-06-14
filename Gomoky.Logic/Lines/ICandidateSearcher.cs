﻿using System.Collections.Generic;

namespace Gomoku.Logic.Lines
{
    public interface ICandidateSearcher
    {
        public IEnumerable<ICoordinates> Search(Game game, int maxDistance = 2, int tolerance = 1);
    }
}