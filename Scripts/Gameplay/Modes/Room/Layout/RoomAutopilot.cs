using Core.Layout.Interactables;
using Core.Layout.PlayerControl;
using Core.PlayerControl;
using Core.PlayerControl.AI;
using Kyzlyk.Collections;
using System.Linq;

namespace Gameplay.Modes.Room.Layout
{
    internal class RoomAutopilot : Autopilot
    {
        public RoomAutopilot(Player player, PlayerLayout playerLayout, InteractablesLayout interactablesLayout) : base(player)
        {
            _playerLayout = playerLayout;
            _interactablesLayout = interactablesLayout;
        }

        private readonly PlayerLayout _playerLayout;
        private readonly InteractablesLayout _interactablesLayout;

        protected override void SetTargets()
        {
            int playersInOtherGroups = GetPlayerCountInOtherGroups();
            if (_targets.Count == playersInOtherGroups + _interactablesLayout.Interactables.Count) 
                return;

            _targets = new ArrayBuilder<ITarget>(playersInOtherGroups + _interactablesLayout.Interactables.Count)
                .Append(_playerLayout.Groups.Where((group, index) => index != Player.GroupIndex))
                .Append(_interactablesLayout.Interactables)
                .ToArray();
        }

        private int GetPlayerCountInOtherGroups()
        {
            int result = 0;
            for (int i = 0; i < _playerLayout.Groups.Count; i++)
            {
                if (i != Player.GroupIndex)
                    result += _playerLayout.Groups[i].Count;
            }

            return result;
        }
    }
}