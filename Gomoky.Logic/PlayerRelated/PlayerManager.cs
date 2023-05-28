using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Gomoku.Logic.PlayerRelated
{
    public class PlayerManager : IDeepCloneable<PlayerManager>
    {
        public PlayerManager(IEnumerable<PlayerRelated.Player> collection)
        {
            var enumerable = collection as PlayerRelated.Player[] ?? collection.ToArray();
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

        public PlayerRelated.Player CurrentPlayer => Players[Turn.Current];

        public ImmutableArray<PlayerRelated.Player> Players { get; }

        public PlayerRelated.Player PreviousPlayer => Players[Turn.Previous];

        public Turn Turn { get; }

        public PlayerRelated.Player this[int index] => Players[index];

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