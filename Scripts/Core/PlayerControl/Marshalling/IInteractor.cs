using Core.PlayerControl.Core;

namespace Core.PlayerControl.Marshalling
{
    public interface IInteractor
    {
        bool IsImpactToStatus { get; }

        void Exclude(params CoreBehaviour[] exclusions);
        bool TryInteract(CoreBehaviour target);
    }
}
