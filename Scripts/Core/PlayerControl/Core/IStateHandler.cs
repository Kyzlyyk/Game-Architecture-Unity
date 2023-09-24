namespace Core.PlayerControl.Core
{
    internal interface IBehaviourHandler
    {
        void OnCollidedWithPlayer(Player other);
    }
}
