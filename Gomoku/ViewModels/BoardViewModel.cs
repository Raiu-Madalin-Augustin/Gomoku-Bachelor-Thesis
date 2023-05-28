using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Gomoku.Logic;

namespace Gomoku.GUI.ViewModels
{
  public class BoardViewModel
  {
    public BoardViewModel(Game game)
    {
      Board = game.Board;

      TileVMs = new TileViewModel[Board.Width, Board.Height];
      for (var i = 0; i < Board.Width; i++)
      {
        for (var j = 0; j < Board.Height; j++)
        {
          TileVMs[i, j] = new TileViewModel(Board[i, j]);
        }
      }

      HighlightedTiles = new ObservableCollection<TileViewModel>();
      HighlightedTiles.CollectionChanged += HighlightedTilesCollectionChanged!;
      game.BoardChanged += GameBoardChanged!;
      game.GameOver += GameOver!;
    }

    public Board Board { get; }

    private ObservableCollection<TileViewModel> HighlightedTiles { get; }

    private TileViewModel[,] TileVMs { get; }

    public TileViewModel this[int x, int y] => TileVMs[x, y];

    public void Clear(int x, int y)
    {
      var tileVm = this[x, y];
      tileVm.Piece = (Piece)Pieces.None;
      HighlightedTiles.Remove(tileVm);
    }

    public void ClearHighlightedTiles()
    {
      int count;
      while ((count = HighlightedTiles.Count) > 0)
      {
        HighlightedTiles.RemoveAt(count - 1);
      }
    }

    public void Highlight(IEnumerable<IPositional> positionals)
    {
      foreach (var position in positionals)
      {
        HighlightedTiles.Add(this[position.X, position.Y]);
      }
    }

    public void Highlight(params IPositional[] positionals)
    {
      foreach (var position in positionals)
      {
        HighlightedTiles.Add(this[position.X, position.Y]);
      }
    }

    public void Select(IPositional position)
    {
      this[position.X, position.Y].IsSelected = true;
    }

    public void Set(int x, int y, Piece piece)
    {
      var tileVm = this[x, y];
      tileVm.Piece = piece;
      HighlightedTiles.Add(tileVm);
    }

    private void GameBoardChanged(object sender, BoardChangedEventArgs e)
    {
      ClearHighlightedTiles();

      if (e.RemovedTiles.Count > 0)
      {
        foreach (var tile in e.RemovedTiles)
        {
          Clear(tile.X, tile.Y);
        }

        var lastTile = ((Game)sender).LastMove;
        if (lastTile != null)
        {
          Highlight(lastTile);
        }
      }

      foreach (var tile in e.AddedTiles)
      {
        Set(tile.X, tile.Y, tile.Piece);
      }
    }

    private void GameOver(object sender, GameOverEventArgs e)
    {
    }

    private void HighlightedTilesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Remove or NotifyCollectionChangedAction.Reset:
            {
                if (e.OldItems != null)
                    foreach (TileViewModel item in e.OldItems)
                    {
                        item.IsHighlighted = false;
                    }

                break;
            }
            case NotifyCollectionChangedAction.Add:
            {
                if (e.NewItems != null)
                    foreach (TileViewModel item in e.NewItems)
                    {
                        item.IsHighlighted = true;
                    }
                break;
            }
        }
    }
  }
}