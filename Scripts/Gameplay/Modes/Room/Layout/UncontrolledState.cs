using Core.PlayerControl;
using Core.PlayerControl.Core;

namespace Gameplay.Modes.Room.Layout
{
    public class UncontrolledState : BaseState
    {
        public UncontrolledState(Player player, IStationStateSwitcher stateSwitcher)
           : base(player, stateSwitcher)
        {
        }

        public override void OnCollidedWithPlayer(Player other)
        {
        }

        public override void Start()
        {
            Player.MovementModule.Auto = true;
        }

        public override void Stop()
        {
        }
    }
}