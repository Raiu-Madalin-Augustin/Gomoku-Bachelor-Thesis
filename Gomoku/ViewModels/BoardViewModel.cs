using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomoku.Logic;

namespace Gomoku.GUI.ViewModels
{
    public class BoardViewModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public TileViewModel[,] TileVMs { get; }
        public TileViewModel this[int x, int y] => TileVMs[x, y];
        public Board Board { get; set; }
        private ObservableCollection<TileViewModel> HighlightedTiles { get; }


        public BoardViewModel(Game game)
        {
            Board = game.Board;
            Width = 15;
            Height = 15;
            TileVMs = new TileViewModel[Width, Height];

            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    TileVMs[i, j] = new TileViewModel(Board[i, j]);
                }
            }
            HighlightedTiles = new ObservableCollection<TileViewModel>();
            HighlightedTiles.CollectionChanged += OnHighlightedTilesChanged;
            game.UpdateBoard += UpdateBoard;
            game.ResetBoard += ResetHighlitedTiled;
        }

        private void OnHighlightedTilesChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (TileViewModel item in e.NewItems)
                {
                    item.IsHighlighted = true;
                }
            }
        }

        private void UpdateBoard(object sender, UpdateBoardEvent e)
        {

            TileViewModel tile = this[e.Tile.X, e.Tile.Y];
            tile.Piece = e.Tile.Piece;

            HighlightedTiles.Add(tile);
        }

        private void ResetHighlitedTiled(object? sender, ResetBoardEvent e)
        {
            foreach (TileViewModel item in HighlightedTiles)
            {
                item.IsHighlighted = false;

            }
            HighlightedTiles.Clear();
        }
    }
}
