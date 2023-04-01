using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        private readonly int _boardWidth;
        private readonly int _boardHeight;
        private readonly IEnumerable<Player> _players;

        public MainWindow() :
            this(15, 15,
            new List<Player>()
            {
                new Player("p1", new Piece(Pieces.X), false),
                new Player("p2", new Piece(Pieces.Y), false)
            })
        {

        }
        public BoardViewModel Board { get; set; }
        public Game Game { get; set; }

        public MainWindow(int boardWidth, int boardHeight, IEnumerable<Player> players)
        {
            InitializeComponent();
            Game = new Game(boardHeight, boardWidth, players);
            Board = new BoardViewModel(Game);

            Initialize(boardHeight, boardWidth);
            _boardWidth = boardWidth;
            _boardHeight = boardHeight;
            _players = players;
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
                        DataContext = Board[i, j]
                    };
                    widthPanel.Children.Add(tile);
                }
                //add the row to the vertical stack
                VerticalStackPanel.Children.Add(widthPanel);

            }
        }
        private void TileClick(object? sender, RoutedEventArgs e)
        {
            if (sender is null || Game.IsOver)
            {
                return;
            }

            if (sender is Button { DataContext: TileViewModel tile }) Game.Play(tile.Tile.X, tile.Tile.Y);
        }

        private void Restart(object sender, RoutedEventArgs e)
        {
            Game.RestartGame();
        }
    }
}
