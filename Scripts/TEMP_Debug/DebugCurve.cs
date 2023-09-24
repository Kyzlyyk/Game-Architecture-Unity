#if UNITY_EDITOR 

using UnityEngine;

namespace UnityEditor
{

    public class DebugCurve : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _plot = new();

        void Update()
        {
            //do something...
            float value = Mathf.Sin(Time.time);

            _plot.AddKey(Time.realtimeSinceStartup, value);
        }
    }
}

#endif