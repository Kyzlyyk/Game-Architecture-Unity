using System.Threading.Tasks;

namespace GameEnvironment.Stream
{
    internal interface ITransitor<TData>
    {
        TData Transit();
    }
    
    internal interface IAsyncTransitor<TData>
    {
        Task<TData> TransitAsync();
    }
}
