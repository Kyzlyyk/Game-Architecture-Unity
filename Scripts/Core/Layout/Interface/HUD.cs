using Kyzlyk.GSystems.UI_Building;
using UnityEngine;

namespace Core.Layout.Interface
{
    public sealed class HUD : InterfaceDesigner, ILayoutModule
    {
        [SerializeField] private Controller _controller;
        [Space]
        [SerializeField] private JoystickElement _joystickElement;

        public Joystick Joystick => _joystickElement.Joystick;

        protected override Element[] Elements => _elements;

        public Controller Controller => _controller;

        private Element[] _elements;

        private void Start()
        {
            _joystickElement.Lock();
        }

        public override void RestoreToDefault()
        {
            _joystickElement.Unlock();
        }

        void ILayoutModule.Draw()
        {
            _elements = new Element[1]
            {
                _joystickElement,
            };

            base.Draw();
            UnlockAll();
        }
    }
}