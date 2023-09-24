using UnityEngine;

namespace Gameplay.Modes.Mini
{
    public class MiniModeSwitcher : MonoBehaviour
    {
        [SerializeField] private float _breakBetweenModes;

        [Space]
        [SerializeField] private MiniMode[] _modes;

        public float ElapsedModeTime { get; private set; }
        public float ElapsedBreakTime { get; private set; }

        private MiniMode CurrentMode => _modes[_currentModeIndex];
        
        private int _currentModeIndex;
        private bool _isBreak;

        private void Start()
        {
            if (_modes.Length > 0)
                StartCurrentMode();
        }

        private void Mode_OnEndTransitionEnded(object sender, System.EventArgs e)
        {
            _isBreak = true;
        }

        private void Update()
        {
            if (_currentModeIndex >= _modes.Length) return;

            if (_isBreak)
            {
                BreakHandle();
                return;
            }

            ElapsedModeTime += Time.deltaTime;

            if (ElapsedModeTime >= CurrentMode.Duration)
            {
                CurrentMode.End();
            }
        }

        public void BreakHandle()
        {
            if (ElapsedBreakTime >= _breakBetweenModes)
            {
                ElapsedBreakTime = 0;
                ElapsedModeTime = 0;

                _currentModeIndex++;

                if (_currentModeIndex < _modes.Length)
                    StartCurrentMode();
            }
            else
                ElapsedBreakTime += Time.deltaTime;
        }

        public void StartCurrentMode()
        {
            CurrentMode.Begin();
            CurrentMode.OnEndTransitionEnded += Mode_OnEndTransitionEnded;
        }
    }
}
