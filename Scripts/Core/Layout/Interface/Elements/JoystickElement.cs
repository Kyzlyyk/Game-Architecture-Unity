using UnityEngine;
using UnityEngine.EventSystems;
using Kyzlyk.GSystems.UI_Building;

namespace Core.Layout.Interface
{
    [RequireComponent(typeof(Joystick))]
    public class JoystickElement : Element
    {
        public Joystick Joystick { get; private set; }

        private void Awake()
        {
            Joystick = GetComponent<Joystick>();
        }

        private bool _isLocked;

        public override void Lock()
        {
            if (_isLocked) return;

            if (Joystick.Direction != Vector2.zero)
                Joystick.OnPointerUp(new PointerEventData(EventSystem.current));    

            Joystick.gameObject.SetActive(false);

            _isLocked = true;
        }

        public override void Unlock()
        {
            _isLocked = false;
            Joystick.gameObject.SetActive(true);
        }

        public override void InitRender()
        {
        }
    }
}