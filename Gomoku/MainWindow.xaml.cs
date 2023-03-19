using System.Windows;
using System.Windows.Controls;
using Gomoku.GUI.ViewModels;

namespace Gomoku.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public BoardViewModel Board { get; }

        public MainWindow()
        {
            InitializeComponent();
            Board = new BoardViewModel();

            Initialize(15, 15);
        }

        private void Initialize(int width, int height)
        {
            //Styles
            var columnStyle = Resources["ColumnStyle"] as Style;
            var tileStyle = Resources["TileStyle"] as Style;
            var marginTileStyle = Resources["MarginTileStyle"] as Style;

            var columns = new StackPanel()
            {
                Style = columnStyle
            };

            //Top left tile
            var emptyTile = new Button()
            {
                Style = marginTileStyle,
                Content = ' '
            };

            columns.Children.Add(emptyTile);

            //add tiles with names
            for (var i = 0; i < width; i++)
            {
                var tile = new Button()
                {
                    Style = marginTileStyle,
                    Content = (char)(i + 'a')
                };
                columns.Children.Add(tile);
            }

            //first line to vertical stack
            VerticalStackPanel.Children.Add(columns);

            //Add rows
            for (var i = 0; i < height; i++)
            {
                var widthPanel = new StackPanel()
                {
                    Style= columnStyle
                };

                var rowCoordinate = new Button()
                {
                    Style = marginTileStyle,
                    Content = i + 1
                };

                widthPanel.Children.Add(rowCoordinate);

                //Add buttons for the tiles for the row
                for (var j = 0; j < width; j++)
                {
                    var tile = new Button()
                    {
                        Style = tileStyle,
                        DataContext = Board[j, i]
                    };
                    widthPanel.Children.Add(tile);
                }
                //add the row to the vertical stack
                VerticalStackPanel.Children.Add(widthPanel);

            }
        }
    }
}
