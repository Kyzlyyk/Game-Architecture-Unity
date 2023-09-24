using Core.PlayerControl.Marshalling;
using Kyzlyk.Core;
using System;

namespace Core.PlayerControl.Lab
{
    public interface IAbilityExecutor
    {
        IAbilityCover<IAbilityExecutor> Cover { get; }
        IInteractor Interactor { get; }

        event EventHandler<IAbilityExecutor, Player> OnInteracted;
        event EventHandler<IAbilityExecutor, EventArgs> OnExecuted;
        
        void Execute(Player anchorPlayer);
    }
}