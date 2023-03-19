using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Gomoku.GUI.ViewModels;
using Gomoku.Logic;

namespace Gomoku.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() :
            this(15, 15,
            new List<Player>()
            {
                new Player("p1", new Piece(), new GomokuBase(), false),
                new Player("p2", new Piece(), new GomokuBase(), false)
            })
        {

        }
        public BoardViewModel Board { get; }
        public Game Game { get; set; }

        public MainWindow(int boardWidth, int boardHeight, IEnumerable<Player> players)
        {
            InitializeComponent();
            Game = new Game(15, 15, players);
            Board = new BoardViewModel(Game);

            Initialize(boardWidth, boardHeight);
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
                    Style = columnStyle
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
