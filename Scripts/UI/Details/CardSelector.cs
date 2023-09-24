using UnityEngine;
using Kyzlyk.Helpers.Extensions;
using Core.PlayerControl.Lab.Details;

namespace GameEnvironment.Lab.UI
{
    internal class CardSelector : MonoBehaviour
    {
        [SerializeField] private DetailsMarshal _detailsMarshal;
        
        [Space]
        [SerializeField] private Transform _playerPreview;
        [SerializeField] private DetailCardCore _cardPrefab;
        [SerializeField] private float _gap;

        private void Start()
        {
            Fill();
        }

        private void Fill()
        {

            int craftedDetailsCount = _detailsMarshal.CraftedCovers.Count;
            if (craftedDetailsCount == 0) return;
            
            Vector3 start = _cardPrefab.transform.localPosition;
            int rightCount, leftCount;

            if (!craftedDetailsCount.IsEven())
            {
                rightCount = Mathf.CeilToInt(craftedDetailsCount / 2f);
                leftCount = Mathf.FloorToInt(craftedDetailsCount / 2f);
            }
            else
            {
                leftCount = rightCount = craftedDetailsCount / 2;
            }

            void CreateCardCore(int craftedDetailIndex, float gap)
            {
                var core = DetailCardCore.Create
                    (
                        prefab: _cardPrefab, 
                        position: new Vector3(start.x += gap, start.y), 
                        rotation: Quaternion.identity, 
                        marshal: _detailsMarshal, 
                        card: _detailsMarshal.CraftedCovers[craftedDetailIndex]
                    );
                
                core.transform.SetParent(transform, false);
            }

            for (int i = 0; i < rightCount; i++)
            {
                CreateCardCore(i, _gap);
            }

            start.x = _cardPrefab.transform.localPosition.x;

            for (int i = rightCount; i < leftCount + rightCount; i++)
            {
                CreateCardCore(i, -_gap);
            }
        }

        public void SlideToRight()
        {

        }

        public void SlideToLeft()
        {

        }
    }
}