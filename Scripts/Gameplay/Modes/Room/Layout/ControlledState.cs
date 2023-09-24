using Core.PlayerControl;
using Core.PlayerControl.Core;

namespace Gameplay.Modes.Room.Layout
{
    public class ControlledState : BaseState
    {
        public ControlledState(Player player, IStationStateSwitcher stateSwitcher) 
            : base(player, stateSwitcher)
        {
        }

        public override void OnCollidedWithPlayer(Player other)
        {
        }

        public override void Start()
        {
        }

        public override void Stop()
        {
        }
    }
}