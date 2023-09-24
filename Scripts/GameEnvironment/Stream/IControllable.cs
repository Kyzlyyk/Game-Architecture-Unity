namespace GameEnvironment.Stream
{
    public interface IControllable
    {
        public bool IsActive { get; }

        void Suspend();
        void Continue();
    }
}
