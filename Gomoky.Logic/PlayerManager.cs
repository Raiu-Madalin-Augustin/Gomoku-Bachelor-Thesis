using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Gomoku.Logic
{
    public class PlayerManager : IDeepCloneable<PlayerManager>
    {
        public PlayerManager(IEnumerable<Player> collection)
        {
            var enumerable = collection as Player[] ?? collection.ToArray();
            if (!enumerable.Any() || !enumerable.Any())
            {
                throw new ArgumentException(nameof(collection));
            }

            Players = enumerable.ToImmutableArray();
            Turn = new Turn(Players.Length);
        }

        private PlayerManager(PlayerManager playerManager)
        {
            Players = ImmutableArray.Create(playerManager.Players, 0, playerManager.Players.Length);
            Turn = playerManager.Turn.ShallowClone();
        }

        public Player CurrentPlayer => Players[Turn.Current];

        public ImmutableArray<Player> Players { get; }

        public Player PreviousPlayer => Players[Turn.Previous];

        public Turn Turn { get; }

        public Player this[int index] => Players[index];

        public PlayerManager DeepClone()
        {
            return new PlayerManager(this);
        }

        object IDeepCloneable.DeepClone()
        {
            return DeepClone();
        }
    }
}