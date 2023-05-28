using System;

using Gomoku.Logic;

namespace Gomoku.GUI.ViewModels
{
  /// <summary>
  /// Defines a tile in the board
  /// </summary>
  public class TileViewModel : VMBase
  {
    private bool _isHighlighted;

    private bool _isSelected;

    public TileViewModel(Tile tile)
    {
      Tile = tile ?? throw new ArgumentNullException(nameof(tile));
    }

    public bool IsHighlighted
    {
      get => _isHighlighted;
      set
      {
        if (IsSelected == true && value == false)
        {
          IsSelected = false;
        }
        SetProperty(ref _isHighlighted, value);
      }
    }

    public bool IsSelected
    {
      get => _isSelected;
      set => SetProperty(ref _isSelected, value);
    }

    public Piece Piece
    {
      get => Tile.Piece;
      set
      {
        Tile.Piece = value;
        NotifyPropertyChanged();
      }
    }

    public Tile Tile { get; }
  }
}