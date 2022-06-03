using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierDrawLine : MonoBehaviour
{
    public List<Transform> wayPoint = new List<Transform>();
    public int pointCount = 100;
    private List<Vector3> linePointList;

    void Init()
    {
        linePointList = new List<Vector3>();
        for (int i = 0; i < pointCount; i++)
        {
            var point = Bezier(i / (float)pointCount, wayPoint);
            linePointList.Add(point);
        }
    }

    void Start()
    {
    }

    bool activated;
    public float rotationSpeed = 10;
    private void Update()
    {
        if (activated)
        {
            transform.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
        }
    }

    public void BezierTranslateLookAt(float duration)
    {
        Dalechn.bl_UpdateManager.RunAction("", duration, (time, realTime) =>
        {
            var point = Bezier(time, wayPoint);
            transform.position = point;
            transform.LookAt(point);
        });
    }

    public void BezierTranslateRotate(float duration)
    {
        Dalechn.bl_UpdateManager.RunAction("", duration, (time, realTime) =>
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(-90, -90, 0)),time);
        },null,Dalechn.EaseType.SineInOut);

        Dalechn.bl_UpdateManager.RunAction("", duration, (time, realTime) =>
        {
            var point = Bezier(time, wayPoint);
            transform.position = point;
            transform.Rotate(Vector3.right * 90 * Time.deltaTime, Space.World);
        });
    }

    // 线性
    Vector3 Bezier(Vector3 p0, Vector3 p1, float t)
    {
        return (1 - t) * p0 + t * p1;
    }

    // 二阶曲线
    Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        Vector3 p0p1 = (1 - t) * p0 + t * p1;
        Vector3 p1p2 = (1 - t) * p1 + t * p2;
        Vector3 result = (1 - t) * p0p1 + t * p1p2;
        return result;
    }

    // 三阶曲线
    Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 result;
        Vector3 p0p1 = (1 - t) * p0 + t * p1;
        Vector3 p1p2 = (1 - t) * p1 + t * p2;
        Vector3 p2p3 = (1 - t) * p2 + t * p3;
        Vector3 p0p1p2 = (1 - t) * p0p1 + t * p1p2;
        Vector3 p1p2p3 = (1 - t) * p1p2 + t * p2p3;
        result = (1 - t) * p0p1p2 + t * p1p2p3;
        return result;
    }

    // n阶曲线，递归实现
    public Vector3 VecBezier(float t, List<Vector3> p)
    {
        if (p.Count < 2)
            return p[0];
        List<Vector3> newp = new List<Vector3>();
        for (int i = 0; i < p.Count - 1; i++)
        {
            //Debug.DrawLine(p[i], p[i + 1], Color.yellow);
            Vector3 p0p1 = (1 - t) * p[i] + t * p[i + 1];
            newp.Add(p0p1);
        }
        return VecBezier(t, newp);
    }

    // transform转换为vector3，在调用参数为List<Vector3>的Bezier函数
    public Vector3 Bezier(float t, List<Transform> p)
    {
        if (p.Count < 2)
            return p[0].position;
        List<Vector3> newp = new List<Vector3>();
        for (int i = 0; i < p.Count; i++)
        {
            newp.Add(p[i].position);
        }
        return VecBezier(t, newp); 
    }


    //在scene视图显示
    public void OnDrawGizmos()
    {
        Init();
        Gizmos.color = Color.yellow;
        //Gizmos.DrawLine()
        for (int i = 0; i < linePointList.Count - 1; i++)
        {
            //var point_1 = Bezier(i/(float)pointCount, wayPoint);
            //var point_2 = Bezier((i+1) / (float)pointCount, wayPoint);
            //两种划线方式皆可
            //Gizmos.DrawLine(point_1, point_2);
            Debug.DrawLine(linePointList[i], linePointList[i + 1], Color.yellow);
        }

    }

}
