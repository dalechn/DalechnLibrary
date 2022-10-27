using System.Collections.Generic;
using UnityEngine;

public enum RandomAreaName
{
    None, StaffArea, ShopKeeperArea, WaitArea
}

public class RandomArea : MonoBehaviour
{
    public static Dictionary<string, RandomArea> areaDict = new Dictionary<string, RandomArea>();  //管理随机区域,放static是因为start就要取值,放 awake ShopInfo可能还没初始化,比如wanderarea ?

    public enum _AreaType { Square, Circle }
    public _AreaType type;
    public Color Color = Color.red;
    public RandomAreaName areaName;

    private void Awake()
    {
        if (areaName != RandomAreaName.None)
        {
            areaDict.Add(areaName.ToString(), this);
        }
    }

    private void Start()
    {
        //if (areaName != RandomAreaName.None)
        //{
        //    ShopInfo.Instance.RegistFloor(areaName, this);
        //}
    }

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }


    public Vector3 GetPosition()
    {
        if (type == _AreaType.Square)
        {
            float x = Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2);
            float z = Random.Range(-transform.localScale.z / 2, transform.localScale.z / 2);
            Vector3 v = transform.position + transform.rotation * new Vector3(x, 0, z);
            return v;
        }
        else if (type == _AreaType.Circle)
        {
            Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            return transform.position + transform.rotation * dir * Random.Range(0, GetMaximumScale());
        }

        return transform.position;
    }

    private float GetMaximumScale()
    {
        float scale = Mathf.Max(transform.localScale.x, transform.localScale.y);
        return Mathf.Max(scale, transform.localScale.z);
    }

    private void Update()
    {
        //测试用
        //Vector3 pos = GetPosition();
        //Debug.DrawLine(pos, pos+new Vector3(0,1,0),Color.red,1.0f);
    }

#if UNITY_EDITOR

    void OnDrawGizmos()
    {
        Gizmos.color = Color;

        if (type == _AreaType.Square)
        {
            Vector3 p1 = transform.position + transform.rotation * new Vector3(transform.localScale.x / 2, 0, transform.localScale.z / 2);
            Vector3 p2 = transform.position + transform.rotation * new Vector3(transform.localScale.x / 2, 0, -transform.localScale.z / 2);
            Vector3 p3 = transform.position + transform.rotation * new Vector3(-transform.localScale.x / 2, 0, transform.localScale.z / 2);
            Vector3 p4 = transform.position + transform.rotation * new Vector3(-transform.localScale.x / 2, 0, -transform.localScale.z / 2);

            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p1, p3);
            Gizmos.DrawLine(p2, p4);
            Gizmos.DrawLine(p3, p4);
        }
        else if (type == _AreaType.Circle)
        {
            UnityEditor.Handles.color = Color;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, GetMaximumScale());
        }
    }
#endif
}