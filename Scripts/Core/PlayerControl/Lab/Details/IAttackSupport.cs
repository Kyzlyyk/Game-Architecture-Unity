using Core.PlayerControl.Core;
using Kyzlyk.Helpers;

namespace Core.PlayerControl.Lab.Details
{
    public interface IAttackSupport : IAbilityExecutor
    {
        UnsignPercent Support(CoreBehaviour behaviourToSupport);
    }
}