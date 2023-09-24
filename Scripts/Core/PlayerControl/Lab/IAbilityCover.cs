using UnityEngine;
using Kyzlyk.Enviroment.SaveSystem;

namespace Core.PlayerControl.Lab
{
    public interface IAbilityCover : ISaveable
    {
        IAbilityExecutor Executor { get; }

        Sprite Icon { get; }
        Class Class { get; }
        Rare Rare { get; }
    }
    
    public interface IAbilityCover<out TExecutor> : IAbilityCover where TExecutor : IAbilityExecutor
    {
        TExecutor DirectExecutor => (TExecutor)Executor;
    }
}