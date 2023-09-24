namespace Core.Layout.Interactables
{
    public interface IPowerUpConstructor : ILayoutPartConstructor
    {
        IPowerUpConstructor AddBuffs();
        IPowerUpConstructor AddDebuffs();

        IPowerUpConstructor AddTraps();
    }
}