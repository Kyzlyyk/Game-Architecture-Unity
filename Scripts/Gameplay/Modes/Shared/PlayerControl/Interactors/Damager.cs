using Core.PlayerControl.Core;
using Core.PlayerControl.Marshalling;

namespace Gameplay.Layout.PlayerControl.Interactors
{
    internal sealed class Damager : IInteractor
    {
        public bool IsImpactToStatus => true;

        public void Exclude(params CoreBehaviour[] exclusions)
        {
            throw new System.NotImplementedException();
        }

        public bool TryInteract(CoreBehaviour target)
        {
            return true;
        }
    }
}
