using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomoku.Logic;

namespace Gomoku.GUI.ViewModels
{
    public class TileViewModel
    {
        public Tile Tile { get; }

        public TileViewModel(Tile tile)
        {
            Tile = tile;
        }
    }
}
