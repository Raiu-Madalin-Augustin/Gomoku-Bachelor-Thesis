using System;

namespace Gomoku.Logic;

/// <summary>
/// Represents the objects related to <see cref="Game.UpdateBoard"/> event.
/// </summary>
public class UpdateBoardEvent : EventArgs
{
    public Tile Tile { get; }
    public UpdateBoardEvent(Tile tile)
    {
        Tile = tile;
    }
}