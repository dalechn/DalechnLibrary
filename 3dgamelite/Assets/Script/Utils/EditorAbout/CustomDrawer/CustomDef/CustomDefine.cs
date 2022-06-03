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


[System.Serializable]
public struct RangeFloat3
{
    public float min;
    public float max;
    public float value;

    public RangeFloat3(float minValue, float maxValue,float value)
    {
        min = minValue;
        max = maxValue;
        this.value = value;
    }

    public float Lerp(float t)
    {
        return Mathf.Lerp(min, max, t);
    }
}


[System.Serializable]
public struct RangeInt3
{
    public int min;
    public int max;
    public int value;

    public RangeInt3(int minValue, int maxValue, int value)
    {
        min = minValue;
        max = maxValue;
        this.value = value;
    }
}
