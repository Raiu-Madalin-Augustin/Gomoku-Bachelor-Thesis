using System;
using Gomoku.Logic.AI;
using Gomoku.Logic.BoardRelated;

namespace Gomoku.Logic.PlayerRelated
{
    public class Player
    {
        public Player(string name, Piece piece, MiniMaxBase ai, bool isAuto = false)
        {
            SetName(name);
            Piece = piece;
            AI = ai;
            IsAuto = isAuto;
        }

        /// <summary>
        /// The AI used for this player.
        /// </summary>
        public MiniMaxBase AI { get; set; }

        /// <summary>
        /// If this <see cref="Player"/> will use AI.
        /// </summary>
        public bool IsAuto { get; set; }

        /// <summary>
        /// Name of player.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The piece that this player will have
        /// </summary>
        public Piece Piece { get; }

        /// <summary>
        /// Sets a non-null or whitespace name for <see cref="Player"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="ArgumentException"></exception>
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(name)} must not be null or empty.");
            }

            Name = name;
        }

        public override string ToString()
        {
            return $"{nameof(Name)}={Name}, {nameof(Piece)}={Piece}, {nameof(IsAuto)}={IsAuto}";
        }
    }
}