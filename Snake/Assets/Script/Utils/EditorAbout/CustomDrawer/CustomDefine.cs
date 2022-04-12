using UnityEngine;

[System.Serializable]
public struct RangeFloat
{
    public float min;
    public float max;

    public RangeFloat(float minValue, float maxValue)
    {
        min = minValue;
        max = maxValue;
    }

    public float Lerp(float t)
    {
        return Mathf.Lerp(min, max, t);
    }
}

[System.Serializable]
public struct RangeInt
{
    public int min;
    public int max;

    public RangeInt(int minValue, int maxValue)
    {
        min = minValue;
        max = maxValue;
    }
}
