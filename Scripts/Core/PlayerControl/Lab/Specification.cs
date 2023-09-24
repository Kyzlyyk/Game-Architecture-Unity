using UnityEngine;

namespace Core.PlayerControl.Lab
{
    public abstract class Specification : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private Color _color;

        public Sprite Icon => _icon;
        public Color Color => _color;

        public int DynamicID { get; private set; }

        private static int s_total;

        private void Awake()
        {
            DynamicID = s_total;
            s_total++;
        }
    }
}
