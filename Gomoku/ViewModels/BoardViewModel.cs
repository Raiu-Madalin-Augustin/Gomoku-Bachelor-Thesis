using System;
using System.Collections.Generic;
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
        }

    }
}
