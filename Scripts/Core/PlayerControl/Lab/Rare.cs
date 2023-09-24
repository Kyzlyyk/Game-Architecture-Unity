using UnityEngine;

namespace Core.PlayerControl.Lab
{
    [CreateAssetMenu(menuName = SharedConstants.Menu_PlayerControl + "/Rare")]
    public sealed class Rare : Specification
    {
        [SerializeField] byte _coeficient;

        public byte Coeficient => _coeficient;

        public int ConvertRareCoeficientToPercentage()
        {
            return 100 / (byte.MaxValue / _coeficient);    
        }
    }
}