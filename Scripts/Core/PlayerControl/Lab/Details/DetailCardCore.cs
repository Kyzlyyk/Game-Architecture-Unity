using UnityEngine;
using UnityEngine.UI;

namespace Core.PlayerControl.Lab.Details
{
    internal class DetailCardCore : MonoBehaviour
    {
        public DetailsMarshal Marshal { get; private set; }
        public DetailCard Card { get; private set; }

        [SerializeField] private Image _detailImage;

        public static DetailCardCore Create(DetailCardCore prefab, Vector3 position, Quaternion rotation, DetailsMarshal marshal, DetailCard card)
        {
            DetailCardCore core = Instantiate(prefab, position, rotation);

            core.Marshal = marshal;
            core.Card = card;

            core._detailImage.sprite = core.Card.Icon;

            return core;
        }

        public void Equip()
        {
            Marshal.Equip_Internal(Card);
        }
    }
}