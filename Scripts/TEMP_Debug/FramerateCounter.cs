using UnityEngine;
using TMPro;

public class FramerateCounter : MonoBehaviour
{
    public TMP_Text TextMeshPro;

    private int _lastFrameIndex;
    private float[] _frameDeltaTimeArray;

    private void Awake()
    {
        _frameDeltaTimeArray = new float[50];
    }

    void Update()
    {
        _frameDeltaTimeArray[_lastFrameIndex] = Time.deltaTime;
        _lastFrameIndex = (_lastFrameIndex + 1) % _frameDeltaTimeArray.Length;

        TextMeshPro.text = Mathf.RoundToInt(CalculateFPS()).ToString();
    }

    private float CalculateFPS()
    {
        float total = 0f;

        foreach (float deltaTime in _frameDeltaTimeArray)
        {
            total += deltaTime;
        }

        return _frameDeltaTimeArray.Length / total;
    }
}
