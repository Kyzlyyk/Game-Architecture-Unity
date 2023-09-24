using Core.PlayerControl;
using Core.PlayerControl.Core;
using Core.PlayerControl.Lab.ShockWaves;
using Core.PlayerControl.Lab.Details;

namespace Gameplay.Modes.Room.Layout
{
    public class RoomCoreBehaviour : CoreBehaviour
    {
        public RoomCoreBehaviour(Player player, bool isControlled, StatusParameter durability, ShockWaveSelector shockWaveSelector, DetailSelector detailSelector)
            : base(player, durability, shockWaveSelector, detailSelector)
        {
            AdjustStates(new BaseState[2]
            {
                new ControlledState(player, this),
                new UncontrolledState(player, this)

            }, isControlled ? 0 : 1);
        }

        public override void Suspend()
        {
            base.Suspend();
            Player.MovementModule.TurnOff();
        }

        public override void Continue()
        {
            base.Continue();
            Player.MovementModule.TurnOn();
        }
    }
}