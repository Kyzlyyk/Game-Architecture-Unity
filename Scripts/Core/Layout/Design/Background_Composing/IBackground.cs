using System.Collections;

namespace Core.Layout.Design.BackgroundComposing
{
    public interface IBackground
    {
        IEnumerator StartTransition();
        IEnumerator EndTransition();
    }
}