using System.Threading.Tasks;

namespace GameEnvironment.Stream
{
    internal sealed class Adapter<TSharedData>
    {
        public Adapter(ITransitor<TSharedData> transitor, ITransitable<TSharedData> transitable)
        {
            _transitable = transitable;
            _transitor = transitor;
        }

        private readonly ITransitable<TSharedData> _transitable;
        private readonly ITransitor<TSharedData> _transitor;

        public void Process()
        {
            _transitable.Take(_transitor.Transit());
        }
        
        public async Task ProcessAsync()
        {
            if (_transitor is IAsyncTransitor<TSharedData> asyncTransitor)
                _transitable.Take(await asyncTransitor.TransitAsync());
            else
                throw new System.InvalidCastException("'Transitor' must be of type 'IAsyncTransitor<T>' for this operation!");
        }
        
        public static void Process(ITransitor<TSharedData> transitor, ITransitable<TSharedData> transitable)
        {
            transitable.Take(transitor.Transit());
        }
        
        public static async Task ProcessAsync(IAsyncTransitor<TSharedData> asyncTransitor, ITransitable<TSharedData> transitable)
        {
            transitable.Take(await asyncTransitor.TransitAsync());
        }
    }
}