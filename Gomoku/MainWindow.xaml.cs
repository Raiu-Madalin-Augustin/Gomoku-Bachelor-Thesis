using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Gomoku.GUI.ViewModels;
using Gomoku.Logic;
using Gomoku.Logic.AI;

namespace Gomoku.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly int _boardWidth;
        private readonly int _boardHeight;
        private readonly IEnumerable<Player> _players;
        private int _secondPlayerScore;
        private int _firstPlayerScore;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string FirstPlayerName { get; set; }

        public string SecondPlayerName { get; set; }

        public int FirstPlayerScore
        {
            get => _firstPlayerScore;
            set
            {
                if (_firstPlayerScore != value)
                {
                    _firstPlayerScore = value;
                    OnPropertyChanged(nameof(FirstPlayerScore));
                }
            }
        }

        public int SecondPlayerScore
        {
            get => _secondPlayerScore;
            set
            {
                if (_secondPlayerScore != value)
                {
                    _secondPlayerScore = value;
                    OnPropertyChanged(nameof(SecondPlayerScore));
                }
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow() :
            this(15, 15,
            new List<Player>()
            {
                new Player("P1", new Piece(Pieces.X), false),
                new Player("P2", new Piece(Pieces.Y),new MinMax(), false)
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

            Game.MoveMade += Board_BoardChangedAsync;


            Initialize(boardHeight, boardWidth);
            _boardWidth = boardWidth;
            _boardHeight = boardHeight;
            _players = players;
            FirstPlayerName = players.First().PlayerName;
            SecondPlayerName = players.Last().PlayerName;
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

            FirstPlayerScore = Game.FirstPlayerScore;
            SecondPlayerScore = Game.SecondPlayerScore;
        }

        private void Restart(object sender, RoutedEventArgs e)
        {
            Storyboard fadeInStoryboard = (Storyboard)RestartGame.Resources["FadeInStoryboard"];
            Storyboard fadeOutStoryboard = (Storyboard)RestartGame.Resources["FadeOutStoryboard"];

            // Start the fade-in animation on the grid
            fadeInStoryboard.Begin(myGrid);

            // Delay the fade-out animation by 1.5 seconds
            Task.Delay(1500).ContinueWith(_ =>
            {
                // Start the fade-out animation on the grid
                fadeOutStoryboard.Begin(myGrid);
            }, TaskScheduler.FromCurrentSynchronizationContext());

            Game.RestartGame();
        }

        private async void Board_BoardChangedAsync(object sender, MoveMadeEvent e)
        {
            // AI
            if (Game is { IsOver: false, CurrentPlayer.GomokuAi: { } })
            {
                await RunAI();
            }
        }

        private async Task RunAI()
        {
            Tuple<int, int> tile = await AIPlayAsync();
            if (tile is null)
            {
                return;
            }
            Game.Play(tile.Item1, tile.Item2);
        }
        private async Task<Tuple<int, int>?> AIPlayAsync(bool showAnalysis = false)
        {
            var player = Game.CurrentPlayer;


            var sw = Stopwatch.StartNew();
            var result = await Task.Run(() => player.GomokuAi?.Analyze(Game));
            sw.Stop();
            if (sw.ElapsedMilliseconds < 500)
            {
                var delay = 500 - sw.ElapsedMilliseconds;
                await Task.Delay((int)delay);
            }


            return result;
        }
    }
}
