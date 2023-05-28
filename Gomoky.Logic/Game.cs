using System;
using System.Collections.Generic;
using System.Linq;

namespace Gomoku.Logic
{
    /// <summary>
    /// Defines a Gomoku game
    /// </summary>
    public class Game : IDeepCloneable<Game>, IShallowCloneable<Game>
    {
        public static readonly int WinCondition = 5;
        private readonly Stack<Tile> _history;

        public Game(int width, int height, IEnumerable<Player> players)
        {
            Board = new Board(width, height);
            MaxMove = width * height;
            Manager = new PlayerManager(players);
            _history = new Stack<Tile>();
            IsOver = false;
            ShiftPlayersOnGameOver = true;
        }

        private Game(Game g)
        {
            if (g is null)
            {
                throw new ArgumentNullException(nameof(g));
            }

            Board = g.Board.DeepClone();
            MaxMove = g.MaxMove;
            Manager = g.Manager.DeepClone();
            _history = new Stack<Tile>(g._history.Reverse());
            IsOver = g.IsOver;
            ShiftPlayersOnGameOver = g.ShiftPlayersOnGameOver;
        }

        public event EventHandler<BoardChangedEventArgs>? BoardChanged;

        public event EventHandler<BoardChangingEventArgs>? BoardChanging;

        public event EventHandler<GameOverEventArgs>? GameOver;

        public Board Board { get; }

        public bool CanUndo => _history.Count > 0;

        public IReadOnlyList<Tile> History => _history.ToArray();

        public bool IsOver { get; private set; }

        public bool IsTie => _history.Count == MaxMove;

        public Tile LastMove => (_history.Count == 0 ? null : _history.Peek())!;

        public PlayerManager Manager { get; }

        public int MaxMove { get; }

        public bool ShiftPlayersOnGameOver { get; set; }

        public bool CheckGameOver(int x, int y)
        {
            if (IsOver || IsTie)
            {
                return true;
            }

            if (_history.Count < 9)
            {
                return false;
            }

            var tile = Board[x, y];

            if (tile.Piece.Type != Pieces.None)
            {
                var orientations = new[]
                {
                      Orientations.Horizontal,
                      Orientations.Vertical,
                      Orientations.Diagonal,
                      Orientations.SecondDiagonal
                };

                foreach (var orientation in orientations)
                {
                    var line = OrientedlLine.FromBoard(
                      Board,
                      tile.X,
                      tile.Y,
                      tile.Piece,
                      orientation,
                      maxTile: WinCondition,
                      blankTolerance: 0);

                    if (!line.IsChained
                        || line.SameTileCount + 1 != WinCondition
                        || line.BlockTilesCount >= 2) continue;
                    return true;
                }
            }
            return false;
        }

        public Game DeepClone()
        {
            return new Game(this);
        }

        public void Play(IPositional positional)
        {
            Play(positional.X, positional.Y);
        }

        public void Play(int x, int y)
        {
            if (IsOver)
            {
                return;
            }

            var tile = Board[x, y];

            if (tile.Piece.Type != Pieces.None)
            {
                return;
            }

            var oldPlayer = Manager.CurrentPlayer;
            tile.Piece = oldPlayer.Piece;
            var previousTile = LastMove;
            _history.Push(tile);

            BoardChanging?.Invoke(
              this,
              new BoardChangingEventArgs(
                new Tile[] { tile },
                Array.Empty<Tile>()));

            if (CheckGameOver(x, y))
            {
                IsOver = true;

                if (ShiftPlayersOnGameOver)
                {
                    Manager.Turn.ShiftStartForwards();
                }
            }

            Manager.Turn.MoveNext();

            BoardChanged?.Invoke(
              this,
              new BoardChangedEventArgs(
                new Tile[] { tile },
                Array.Empty<Tile>()));

            if (IsOver)
            {
                GameOver?.Invoke(
                this,
                new GameOverEventArgs(
                  Manager.Turn.Current,
                  oldPlayer));
            }
        }

        public void Restart()
        {
            var history = _history.ToArray();
            BoardChanging?.Invoke(
              this,
              new BoardChangingEventArgs(
                Array.Empty<Tile>(),
                history));

            foreach (var tile in _history)
            {
                tile.Piece = new Piece(Pieces.None);
            }

            Manager.Turn.Reset();
            _history.Clear();
            IsOver = false;

            BoardChanged?.Invoke(
              this,
              new BoardChangedEventArgs(
                Array.Empty<Tile>(),
                history));
        }

        public Game ShallowClone()
        {
            return (Game)MemberwiseClone();
        }

        public void Undo()
        {
            if (!CanUndo)
            {
                return;
            }

            var removedTile = _history.Pop();
            BoardChanging?.Invoke(
              this,
              new BoardChangingEventArgs(
                Array.Empty<Tile>(),
                new Tile[] { removedTile }));

            removedTile.Piece = new Piece(Pieces.None);
            Manager.Turn.MoveBack();
            if (IsOver)
            {
                IsOver = false;
                Manager.Turn.ShiftStartBackwards();
            }

            BoardChanged?.Invoke(
              this,
              new BoardChangedEventArgs(
                Array.Empty<Tile>(),
                new Tile[] { removedTile }));
        }

        object IDeepCloneable.DeepClone()
        {
            return DeepClone();
        }
        object IShallowCloneable.ShallowClone()
        {
            return ShallowClone();
        }
    }
}