using System;
using System.Collections.Generic;

namespace Gomoku.Logic.BoardRelated
{
    public class BoardChangingEventArgs : EventArgs
    {
        public BoardChangingEventArgs(
          IList<Tile> willBeAddedTiles,
          IList<Tile> willBeRemovedTiles)
        {
            WillBeAddedTiles = willBeAddedTiles ?? throw new ArgumentNullException(nameof(willBeAddedTiles));
            WillBeRemovedTiles = willBeRemovedTiles ?? throw new ArgumentNullException(nameof(willBeRemovedTiles));
        }

        public IList<Tile> WillBeAddedTiles { get; }
        public IList<Tile> WillBeRemovedTiles { get; }
    }
}