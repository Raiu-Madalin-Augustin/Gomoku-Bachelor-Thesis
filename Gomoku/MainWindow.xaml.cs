using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Gomoku.GUI.ViewModels;
using Gomoku.Logic;
using Gomoku.Logic.AI;
using Gomoku.Logic.BoardRelated;
using Gomoku.Logic.PlayerRelated;

namespace Gomoku.GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow() :
          this(15, 15,
            new List<Player>()
            {
          new Player("Player 1", new Piece(Pieces.X), new PythonMiniMax()),
          new Player("Player 2", new Piece(Pieces.O), new MiniMax(), true),
            })
        {
        }

        public MainWindow(int boardWidth, int boardHeight, IEnumerable<Player> players)
        {
            InitializeComponent();

            Game = new Game(boardWidth, boardHeight, players);
            BoardViewModel = new BoardViewModel(Game);
            InitializeBoard(boardWidth, boardHeight);
            Game.BoardChanged += BoardChangedAsync!;
            Game.GameOver += BoardGameOver!;
        }

        public BoardViewModel BoardViewModel { get; }
        public Game Game { get; }

        private async Task<ICoordinates> AiPlayAsync(bool showAnalysis = false)
        {
            var player = Game.Manager.CurrentPlayer;

            var sw = Stopwatch.StartNew();
            var result = await Task.Run(() => player.AI.Analyze(Game));
            sw.Stop();
            if (sw.ElapsedMilliseconds < 500)
            {
                var delay = 500 - sw.ElapsedMilliseconds;
                await Task.Delay((int)delay);
            }

            if (!showAnalysis) return result.SelectedChoice;

            BoardViewModel.ClearHighlightedTiles();
            BoardViewModel.Highlight(result.PossibleChoices);
            BoardViewModel.Highlight(Game.LastMove);

            return result.SelectedChoice;
        }

        private async void AnalyzeButtonClick(object sender, RoutedEventArgs e)
        {
            if (Game.IsOver)
            {
                return;
            }

            var selectedTile = await AiPlayAsync(showAnalysis: true);
            BoardViewModel.Select(selectedTile);
        }

        private async void BoardChangedAsync(object sender, BoardChangedEventArgs e)
        {
            if (Game is { IsOver: false, Manager.CurrentPlayer.IsAuto: true }
                && UseAIToggleButton.IsChecked == true)
            {
                await RunAi();
            }
        }

        private void BoardGameOver(object sender, GameOverEventArgs e)
        {
            MessageTextBlock.Text = e.Winner is null ? "Tie!" : $"{e.Winner.Name} wins!";
            MessageGrid.Visibility = Visibility.Visible;
            DemoToggleButton.IsChecked = false;
        }

        private async void AiVsAiToggleButtonChecked(object sender, RoutedEventArgs e)
        {
            if (Game.IsOver)
            {
                RestartButtonClick(null!, null!);
            }

            foreach (var player in Game.Manager.Players)
            {
                player.IsAuto = true;
            }

            UseAIToggleButton.IsChecked = true;
            UseAIToggleButton.IsEnabled = false;
            AnalyzeButton.IsEnabled = false;
            RestartButton.IsEnabled = false;
            UndoButton.IsEnabled = false;

            await RunAi();
        }

        private void AiVsAiToggleButtonUnchecked(object sender, RoutedEventArgs e)
        {
            UseAIToggleButton.IsChecked = false;
            UseAIToggleButton.IsEnabled = true;
            AnalyzeButton.IsEnabled = true;
            RestartButton.IsEnabled = true;
            UndoButton.IsEnabled = true;
            Game.Manager.Players.First().IsAuto = false;
        }

        private void InitializeBoard(int width, int height)
        {
            // Gets horizontal stack style resource
            var widthStackPanelStyle = Resources["WidthStackPanelStyle"] as Style;

            // Gets clickable tile style resource
            var tileStyle = Resources["TileButtonStyle"] as Style;

            // Gets non-clickable tile style resource
            var coorTileStlye = Resources["CoordinateTileButtonStyle"] as Style;

            // Creates first row (coordinates row that has alphabetic characters)
            var columnStackPanel = new StackPanel
            {
                Style = widthStackPanelStyle
            };

            // Add blank tile (top-left tile)
            var blankTile = new Button
            {
                Style = coorTileStlye,
                Content = ' ',
            };
            columnStackPanel.Children.Add(blankTile);

            // Add the rest of coordinate tiles with names
            for (var j = 0; j < width; j++)
            {
                var coordinateButton = new Button
                {
                    Style = coorTileStlye,
                    Content = j,
                };
                columnStackPanel.Children.Add(coordinateButton);
            }

            // Add first line to the vertical stack
            HeightStackPanel.Children.Add(columnStackPanel);

            // Add the rest of rows
            for (var i = 0; i < height; i++)
            {
                // Creates new horizontal stack representing a row
                var widthStackPanel = new StackPanel
                {
                    Style = widthStackPanelStyle
                };

                // Add coordinate button (left-most column)
                var coordinateButton = new Button
                {
                    Style = coorTileStlye,
                    Content = i

                };

                // Add the coordinate button to the horizontal stack
                widthStackPanel.Children.Add(coordinateButton);

                // Add the rest of clickable buttons to the stack
                for (var j = 0; j < width; j++)
                {
                    var tileButton = new Button
                    {
                        DataContext = BoardViewModel[j, i],
                        Style = tileStyle
                    };
                    widthStackPanel.Children.Add(tileButton);
                }

                // Add the horizontal stack to the vertical stack
                HeightStackPanel.Children.Add(widthStackPanel);
            }
        }

        private void MessageGridPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MessageGrid.Visibility != Visibility.Collapsed)
            {
                MessageGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void RestartButtonClick(object sender, RoutedEventArgs e)
        {
            Game.Restart();
            MessageGridPreviewMouseDown(null!, null!);
            DemoToggleButton.IsChecked = false;
        }

        private async Task RunAi()
        {
            var tile = await AiPlayAsync();
            if (tile is null)
            {
                return;
            }
            Game.Play(tile.X, tile.Y);
        }

        private void TileButtonClick(object? sender, RoutedEventArgs e)
        {
            if (sender is null || Game.IsOver)
            {
                return;
            }

            if (sender is not Button button
                || button.DataContext is null) return;
            if (button.DataContext is TileViewModel tileVm) Game.Play(tileVm.Tile.X, tileVm.Tile.Y);
        }

        private void UndoButtonClick(object sender, RoutedEventArgs e)
        {
            Game.Undo();
        }

        private async void UseAiToggleButtonChecked(object sender, RoutedEventArgs e)
        {
            if (!Game.IsOver
              && DemoToggleButton.IsChecked == false
              && Game.Manager.CurrentPlayer.IsAuto
              && UseAIToggleButton.IsChecked == true)
            {
                await RunAi();
            }
        }
    }
}
