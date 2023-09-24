namespace Core.PlayerControl.Core
{
    public abstract class BaseState : IBehaviourHandler
    {
        public BaseState(Player player, IStationStateSwitcher stateSwitcher)
        {
            Player = player;
            StateSwitcher = stateSwitcher;
        }

        protected Player Player { get; }
        protected CoreBehaviour Core { get; }
        protected IStationStateSwitcher StateSwitcher { get; }

        public abstract void OnCollidedWithPlayer(Player other);

        public abstract void Start();
        public abstract void Stop();
    }
}
