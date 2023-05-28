using System;

namespace Gomoku.Logic
{
    /// <summary>
    /// Represents the game turn
    /// </summary>
    public class Turn : IShallowCloneable<Turn>
    {
        /// <summary>
        /// Instantiate <see cref="Turn"/> with max turn.
        /// </summary>
        /// <param name="maxTurn">the maximum turn can be reached</param>
        /// <param name="start">the starting turn, defaults to 0</param>
        public Turn(int maxTurn, int start = 0)
        {
            if (maxTurn < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxTurn));
            }

            Max = maxTurn;
            IsReverse = false;
            Start = start;
            Current = start;
        }

        /// <summary>
        /// The current turn.
        /// </summary>
        public int Current { get; private set; }

        /// <summary>
        /// If the turn order is reversed.
        /// </summary>
        public bool IsReverse { get; set; }

        /// <summary>
        /// The maximum turn can be reached
        /// </summary>
        public int Max { get; }

        /// <summary>
        /// The next turn, taking into account of <see cref="IsReverse"/>.
        /// </summary>
        public int Next => GetNext(Current, IsReverse);

        /// <summary>
        /// The previous turn, taking into account of <see cref="IsReverse"/>.
        /// </summary>
        public int Previous => GetNext(Current, IsReverse);

        /// <summary>
        /// The start of the turn.
        /// </summary>
        public int Start { get; private set; }

        /// <summary>
        /// Moves <see cref="Current"/> to previous. If <see cref="Current"/>
        /// reaches 0, it goes back to <see cref="Max"/> - 1.
        /// </summary>
        public void MoveBack()
        {
            Current = Previous;
        }

        /// <summary>
        /// Moves <see cref="Current"/> to next. If <see cref="Current"/> reaches
        /// <see cref="Max"/>, it goes back to 0.
        /// </summary>
        public void MoveNext()
        {
            Current = Next;
        }

        /// <summary>
        /// Resets <see cref="Current"/> to <see cref="Start"/>.
        /// </summary>
        public void Reset()
        {
            Current = Start;
        }

        /// <summary>
        /// Reverses the order of the turn.
        /// </summary>
        public void Reverse()
        {
            IsReverse = !IsReverse;
        }

        public void ShiftStartBackwards()
        {
            Start = GetNext(Start, true);
        }
        public void ShiftStartForwards()
        {
            Start = GetNext(Start, false);
        }

        private int GetNext(int from, bool isReverse)
        {
            var OrderModifier = isReverse ? -1 : 1;
            return ((from + 1) * OrderModifier + Max) % Max;
        }

        object IShallowCloneable.ShallowClone()
        {
            return ShallowClone();
        }
        public Turn ShallowClone()
        {
            return (Turn)MemberwiseClone();
        }
    }
}