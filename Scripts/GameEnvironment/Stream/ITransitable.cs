namespace GameEnvironment.Stream
{
    internal interface ITransitable<TData>
    {
        void Take(TData data);
    }
}