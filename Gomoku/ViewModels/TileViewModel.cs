using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomoku.Logic;

namespace Gomoku.GUI.ViewModels
{
    public class TileViewModel : ViewModelBase
    {
        private bool _isSelected;
        private bool _isHighlighted;
        public Tile Tile { get; }

        public bool IsHighlighted
        {
            get => _isHighlighted;
            set => Set(ref _isHighlighted, value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (IsSelected && value == false)
                {
                    IsSelected = false;
                }

                Set(ref _isSelected, value);
            }
        }

        public TileViewModel(Tile tile)
        {
            Tile = tile;
        }
    }
}
