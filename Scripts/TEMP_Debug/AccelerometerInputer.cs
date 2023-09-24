#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor
{
    public class AccelerometerInputer : MonoBehaviour
    {
        void OnGUI()
        {
            GUIStyle style = new()
            {
                fontSize = 18,
                richText = true,
            };

            GUI.Label(new Rect(10f, 50f, 200f, 40f), "<color=white>X: " + Input.acceleration.x + "</color>", style);
            GUI.Label(new Rect(10f, 75f, 200f, 40f), "<color=white>Y: " + Input.acceleration.y + "</color>", style);
            GUI.Label(new Rect(10f, 100f, 200f, 40f), "<color=white>Z: " + Input.acceleration.z + "</color>", style);
        }
    }
}
#endif