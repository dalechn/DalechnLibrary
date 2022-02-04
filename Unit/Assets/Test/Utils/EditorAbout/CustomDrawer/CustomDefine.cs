using System.Collections.Generic;
using UnityEngine;

namespace CJTools
{
    [System.Serializable]
    public struct CheckVector3
    {
        public bool check;
        public Vector3 value;
        public Vector3 Minimum;
        public Vector3 Maximum;

        public void Range()
        {
            if (check)
            {
                value += GameUtils.Range(Minimum, Maximum);
            }
        }
    }

    [System.Serializable]
    public struct CheckFloat
    {
        public bool check;
        public float value;
        public float Minimum;
        public float Maximum;

        public void Range()
        {
            if (check)
            {
                value += Random.Range(Minimum, Maximum);
            }
        }
    }

    [System.Serializable]
    public struct CheckInt
    {
        public bool check;
        public int value;
        public int Minimum;
        public int Maximum;

        public void Range()
        {
            if (check)
            {
                value += Random.Range(Minimum, Maximum);
            }
        }
    }

    [System.Serializable]
    public struct DoubleVector
    {
        public Vector3 Minimum;
        public Vector3 Maximum;
    }

    [System.Serializable]
    public struct DoubleGameObject
    {
        public GameObject Origin;
        public GameObject Dest;
    }

    [System.Serializable]
    public struct DoubleInt
    {
        public int Minimum;
        public int Maximum;
    }

    [System.Serializable]
    public struct DoubleFloat
    {
        public float Minimum;
        public float Maximum;
    }

    [System.Serializable]
    public struct DoubleBool
    {
        public bool Minimum;
        public bool Maximum;
    }

    [System.Serializable]
    public struct ThreeInt
    {
        public int Minimum;
        public int Maximum;
        public int Value;
    }

    [System.Serializable]
    public struct ThreeFloat
    {
        public float Minimum;
        public float Maximum;
        public float Value;
    }
    [System.Serializable]
    public class RRangeGameObjectList : ReorderableList<DoubleGameObject> { }
    [System.Serializable]
    public class RTransformList : ReorderableList<Transform> { }
    [System.Serializable]
    public class RGameObjectList : ReorderableList<GameObject> { }
    [System.Serializable]
    public class RParticleList : ReorderableList<ParticleSystem> { }
    [System.Serializable]
    public class RStringList : ReorderableList<string> { }
    [System.Serializable]
    public class RVectorList : ReorderableList<Vector3> { }

    // inherit this
    public class ReorderableList<T> : ReorderableListBase
    {
        public List<T> List;
    }

    public class ReorderableListBase { }

    // 编辑器属性
    public class SingleLineAttribute : PropertyAttribute
    {
        public SingleLineAttribute(string tooltip) { Tooltip = tooltip; }
        public SingleLineAttribute() { }

        public string Tooltip { get; private set; }
    }

    public class SingleLineClampAttribute : SingleLineAttribute
    {
        public SingleLineClampAttribute(string tooltip, double minValue, double maxValue)
            : base(tooltip)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
        public SingleLineClampAttribute(double minValue, double maxValue)
            : base()
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public double MinValue { get; private set; }
        public double MaxValue { get; private set; }
    }
}
