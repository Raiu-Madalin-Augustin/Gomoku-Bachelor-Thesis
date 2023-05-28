namespace Gomoku.Logic.PlayerRelated
{
    public class Turn : IShallowCloneable<Turn>
    {
        public Turn(int maxTurn, int start = 0)
        {
            Max = maxTurn;
            IsReverse = false;
            Start = start;
            Current = start;
        }

        public int Current { get; private set; }

        public bool IsReverse { get; set; }

        public int Max { get; }

        public int Next => GetNext(Current, IsReverse);

        public int Previous => GetNext(Current, IsReverse);

        public int Start { get; private set; }

        public void MoveBack()
        {
            Current = Previous;
        }

        public void MoveNext()
        {
            Current = Next;
        }

        public void Reset()
        {
            Current = Start;
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