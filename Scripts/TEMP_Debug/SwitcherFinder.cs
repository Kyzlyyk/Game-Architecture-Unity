#if UNITY_EDITOR

using UnityEngine;
using System.Collections.Generic;
using Kyzlyk.Helpers.Utils;
using Kyzlyk.UI;

namespace UnityEditor
{
    [RequireComponent(typeof(SwitcherButton))]
    public class SwitcherFinder : MonoBehaviour
    {
        public FindNameAlgorithm SearchNameAlgorithm;
        public FindObjectAlgorithm SearchObjectAlgorithm;

        [HideInInspector][SerializeField] private string _customName;
        [HideInInspector][SerializeField] private Transform _searchIn;
        [HideInInspector][SerializeField] private Switcher _switcher;

        [ContextMenu("Find Switcher in Hierarchy")]
        public void Find()
        {
            string searchName = SearchNameAlgorithm switch
            {
                FindNameAlgorithm.GameObjectName => name,
                FindNameAlgorithm.CustomName => _customName,

                _ => string.Empty
            };
            
            bool TryAssignSwitcher(Switcher switcher)
            {
                if (switcher.name == searchName)
                {
                    GetComponent<SwitcherButton>().Switcher = switcher;
                    return true;
                }

                return false;
            }

            void SearchInObject(Transform transform)
            {
                Switcher[] switchers = transform.GetComponentsInChildren<Switcher>(includeInactive: true);

                for (int i = 0; i < switchers.Length; i++)
                {
                    if (TryAssignSwitcher(switchers[i]))
                        return;
                }
            }

            if (SearchObjectAlgorithm == FindObjectAlgorithm.InMyself)
            {
                SearchInObject(transform);
            }
            else if (SearchObjectAlgorithm == FindObjectAlgorithm.InObjectDirect)
            {
                GetComponent<SwitcherButton>().Switcher = _switcher;
            }
            else if (SearchObjectAlgorithm == FindObjectAlgorithm.InParent)
            {
                Switcher[] switchers = transform.parent.GetComponentsInChildren<Switcher>(includeInactive: true);

                for (int i = 0; i < switchers.Length; i++)
                {
                    if (switchers[i] == this) continue;

                    if (TryAssignSwitcher(switchers[i]))
                        return;
                }
            }
            else if (SearchObjectAlgorithm == FindObjectAlgorithm.InObject)
            {
                SearchInObject(_searchIn);
            }
            else if (SearchObjectAlgorithm == FindObjectAlgorithm.InAll)
            {
                IEnumerable<Switcher> allSwitchers = UnityUtility.GetAllObjectsOnScene<Switcher>();
                foreach (var switcher in allSwitchers)
                {
                    if (switcher == this) continue;

                    if (TryAssignSwitcher(switcher))
                        return;
                }
            }
        }

        public enum FindNameAlgorithm
        {
            GameObjectName = 0,
            CustomName = 1,
        }

        public enum FindObjectAlgorithm
        {
            InObjectDirect = 4,
            InObject = 3,
            InMyself = 2,
            InParent = 1,
            InAll = 0,
        }
    }
}

#endif