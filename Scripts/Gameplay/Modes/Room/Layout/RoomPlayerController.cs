using Core.Layout;
using Core.Layout.PlayerControl;
using Core.PlayerControl;
using Kyzlyk.Core;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Modes.Room.Layout
{
    [CreateAssetMenu(menuName = SharedConstants.Menu_PlayerControllers + "/Room")]
    public sealed class RoomPlayerController : Controller
    {
        private PlayerLayout Layout => Singleton<PlayerLayout>.Instance;

        private static int currentPlayer;
        private static int currentGroup;

        private TaskCompletionSource<bool> _waitingUntilContinues = new();

        private Player CurrentPlayer => Layout.Groups[currentGroup][currentPlayer];

        public override void Init()
        {
            for (int i = 0; i < Layout.Groups.Count; i++)
            {
                for (int j = 0; j < Layout.Groups[i].Count; j++)
                {
                    FreezePlayerBehaviour(Layout.Groups[i][j]);
                    Layout.Groups[i][j].MovementModule.OnMovementEnd += MovementModule_OnMovementEnd;
                }
            }

            currentGroup = Layout.MainGroupIndex;
            currentPlayer = Layout.MainIndex;

            UnfreezePlayerBehaviour(CurrentPlayer);

            Continue();
        }

        private void MovementModule_OnMovementEnd(object sender, System.EventArgs e)
        {
            if (ReferenceEquals((IMovementModule)sender, CurrentPlayer.MovementModule))
                NextPlayer();
        }

        private async void NextPlayer()
        {
            await _waitingUntilContinues.Task;

            FreezePlayerBehaviour(CurrentPlayer);

            currentPlayer++;

            if (currentPlayer >= Layout.Groups[currentGroup].Count)
            {
                currentGroup++;

                if (currentGroup >= Layout.Groups.Count)
                    currentGroup = 0;

                currentPlayer = 0;
            }

            UnfreezePlayerBehaviour(CurrentPlayer);
        }

        protected override void SuspendInternal()
        {
            _waitingUntilContinues = new TaskCompletionSource<bool>();
        }

        protected override void ContinueInternal()
        {
            _waitingUntilContinues.SetResult(true);
        }

        private void FreezePlayerBehaviour(Player player)
        {
            player.Behaviour.Suspend();
            player.Behaviour.ShockWaveSelector.Deactivate();
            player.Behaviour.DetailSelector.Deactivate();
        }

        private void UnfreezePlayerBehaviour(Player player)
        {
            player.Behaviour.Continue();
            player.Behaviour.ShockWaveSelector.Activate();
            player.Behaviour.DetailSelector.Activate();
        }

        protected override void ReturnControlInternal()
        {
            IMovementModule movementModule = CurrentPlayer.MovementModule;
            Continue();
            
            if (movementModule == CurrentPlayer.MovementModule && !movementModule.IsMoving)
                NextPlayer();
        }
    }
}