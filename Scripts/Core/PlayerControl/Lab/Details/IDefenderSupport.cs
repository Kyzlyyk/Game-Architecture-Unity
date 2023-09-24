using Core.PlayerControl.Core;
using Kyzlyk.Helpers;

namespace Core.PlayerControl.Lab.Details
{
    public interface IDefenderSupport : IAbilityExecutor
    {
        UnsignPercent Support(CoreBehaviour behaviourToSupport, IAbilityCover attackerInfo);
    }
}