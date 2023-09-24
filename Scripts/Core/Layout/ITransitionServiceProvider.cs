using System.Collections;

namespace Core.Layout
{
    public interface ITransitionServiceProvider
    {
        IEnumerator Begin();
        IEnumerator End();
    }
}