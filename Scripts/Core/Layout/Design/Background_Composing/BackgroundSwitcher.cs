using System.Collections;
using UnityEngine;

namespace Core.Layout.Design.BackgroundComposing
{
    public sealed class BackgroundSwitcher : MonoBehaviour
    {
        private IBackground _currentBackground;

        public void Switch(IBackground background)
        {
            StartCoroutine(Switch_Coroutine(_currentBackground, background));

            _currentBackground = background;
        }

        private IEnumerator Switch_Coroutine(IBackground replace, IBackground switchTo)
        {
            yield return StartCoroutine(replace.EndTransition());

            yield return StartCoroutine(switchTo.StartTransition());
        }
    }
}