namespace AI.Commands
{
    public abstract class Command
    {
        public abstract float Delay { get; }

        public abstract void Execute();
    }
}