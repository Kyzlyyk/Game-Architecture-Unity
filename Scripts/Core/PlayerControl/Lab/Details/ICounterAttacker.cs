namespace Core.PlayerControl.Lab.Details
{
    public interface ICounterAttacker : IAbilityExecutor
    {
        void CounterAttack(IAbilityExecutor aggressor, Player target);
    }
}