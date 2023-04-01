using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Gomoku.Logic
{
    /// <summary>
    /// Represents the objects related to <see cref="Game.UpdateBoard"/> event.
    /// </summary>
    public class UpdateBoardEvent : EventArgs
    {
        public Tile Tile { get; }
        public UpdateBoardEvent(Tile tile)
        {
            Tile = tile;
        }
    }

    public class ResetBoardEvent : EventArgs
    {
        public ResetBoardEvent() { }
    }

    public class Game
    {
        private readonly int _width;
        private readonly int _height;

        public Board Board { get; set; }

        /// <summary>
        /// Checks if the game is over.
        /// </summary>
        public bool IsOver { get; private set; }
        public IEnumerable<Player> Players { get; }
        public Player CurrentPlayer { get; set; }

        public event EventHandler<UpdateBoardEvent>? UpdateBoard;
        public event EventHandler<ResetBoardEvent>? ResetBoard;


        public Game(int width, int height, IEnumerable<Player> players)
        {
            Board = new Board(width, height);
            IsOver = false;
            _width = width;
            _height = height;
            var enumerable = players as Player[] ?? players.ToArray();
            Players = enumerable;
            CurrentPlayer = enumerable.First();
        }

        public void Play(int x, int y)
        {
            if (IsOver)
            {
                return;
            }
            var tile = Board[x, y];

            if (tile.Piece.TypeIndex != ((int)Pieces.None))
            {
                return;
            }

            var currentPlayerData = Players.FirstOrDefault(player => player == CurrentPlayer)!;
            tile.Piece = currentPlayerData.Piece;

            CurrentPlayer = CurrentPlayer == currentPlayerData ? Players.FirstOrDefault(player => player != CurrentPlayer)! : currentPlayerData;
            UpdateBoard?.Invoke(this, new UpdateBoardEvent(tile));

            if (CheckIfGameIsOver(_height, _width, tile.Piece))
            {
                IsOver = true;
            }

            if (IsOver)
            {
                MessageBox.Show("Winner");
            }

            if (CurrentPlayer.GomokuAi != null && CurrentPlayer.GomokuAi.GetType() == typeof(GomokuBase))
            {
                var move = CurrentPlayer.GomokuAi.Analyze();
                Play(move.Item1, move.Item2);
            }
        }


        private bool CheckIfGameIsOver(int x, int y, Pieces piece)
        {

            // vertical
            for (var row = 0; row <= _height - 5; row++)
            {
                for (var col = 0; col < _width; col++)
                {
                    if (Board[row, col].Piece != Pieces.None &&
                    Board[row, col].Piece.TypeIndex == Board[row + 1, col].Piece.TypeIndex &&
                    Board[row, col].Piece.TypeIndex == Board[row + 2, col].Piece.TypeIndex &&
                    Board[row, col].Piece.TypeIndex == Board[row + 3, col].Piece.TypeIndex &&
                    Board[row, col].Piece.TypeIndex == Board[row + 4, col].Piece.TypeIndex)
                    {
                        return true;
                    }
                }
            }

            // horizontal
            for (var row = 0; row < _height; row++)
            {
                for (var col = 0; col <= _width - 5; col++)
                {
                    if (Board[row, col].Piece != Pieces.None &&
                    Board[row, col].Piece.TypeIndex == Board[row, col + 1].Piece.TypeIndex &&
                    Board[row, col].Piece.TypeIndex == Board[row, col + 2].Piece.TypeIndex &&
                    Board[row, col].Piece.TypeIndex == Board[row, col + 3].Piece.TypeIndex &&
                    Board[row, col].Piece.TypeIndex == Board[row, col + 4].Piece.TypeIndex)

                    {
                        return true;
                    }
                }
            }

            // right and down
            for (var row = 0; row <= _height - 5; row++)
            {
                for (var col = 0; col <= _width - 5; col++)
                {
                    if (Board[row, col].Piece != Pieces.None &&
                    Board[row, col].Piece.TypeIndex == Board[row + 1, col + 1].Piece.TypeIndex &&
                    Board[row, col].Piece.TypeIndex == Board[row + 2, col + 2].Piece.TypeIndex &&
                    Board[row, col].Piece.TypeIndex == Board[row + 3, col + 3].Piece.TypeIndex &&
                    Board[row, col].Piece.TypeIndex == Board[row + 4, col + 4].Piece.TypeIndex)
                    {
                        return true;
                    }
                }
            }

            // right and up
            for (var row = 4; row < _height; row++)
            {
                for (var col = 0; col <= _width - 5; col++)
                {
                    if (Board[row, col].Piece != Pieces.None &&
                    Board[row, col].Piece.TypeIndex == Board[row - 1, col + 1].Piece.TypeIndex &&
                    Board[row, col].Piece.TypeIndex == Board[row - 2, col + 2].Piece.TypeIndex &&
                    Board[row, col].Piece.TypeIndex == Board[row - 3, col + 3].Piece.TypeIndex &&
                    Board[row, col].Piece.TypeIndex == Board[row - 4, col + 4].Piece.TypeIndex)
                    {
                        return true;
                    }
                }
            }
            // If no win condition is met, the game is not over
            return false;
        }

        public void RestartGame()
        {
            IsOver = false;
            for (var i = 0; i < _height; i++)
            {
                for (var j = 0; j < _width; j++)
                {
                    var tile = Board[i, j];
                    tile.Piece = new Piece(Pieces.None);
                    UpdateBoard?.Invoke(this, new UpdateBoardEvent(tile));
                }
            }
            ResetBoard?.Invoke(this, new ResetBoardEvent());
        }
    }
}
