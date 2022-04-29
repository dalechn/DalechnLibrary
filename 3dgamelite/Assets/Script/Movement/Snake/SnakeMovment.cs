using System.Collections.Generic;
using UnityEngine;

public struct MovementInfo
{
    public Vector3 pos;
    public Quaternion rot;

    public MovementInfo(Vector3 pos, Quaternion rot)
    {
        this.pos = pos;
        this.rot = rot;
    }
}

public class SnakeMovment : MonoBehaviour
{
    public float speed = 5;
    public float smoothDampTime = 0.3f;
    public int initLength = 3;
    public float tailMargin = 0.03f;
    public AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal;
    public bool continuous;

    public GameObject head;
    public GameObject tailPrefab;

    private List<TailMovment> tailList = new List<TailMovment>();
    private List<MovementInfo> pathList = new List<MovementInfo>();

    private float pathSpacing;

    private void Start()
    {
        AddHead();
        for (int i = 0; i < initLength; i++)
        {
            AddTail();
        }

        pathList.Insert(0,new MovementInfo( head.transform.position,head.transform.rotation));

        // 大于0的固定长度即可
        pathSpacing = Time.fixedDeltaTime;
    }

    private void Update()
    {
        if (updateMode != AnimatorUpdateMode.AnimatePhysics)
        {
            EditPath();
            MoveTail(Time.deltaTime * speed);
        }
    }

    private void FixedUpdate()
    {
        if (updateMode == AnimatorUpdateMode.AnimatePhysics)
        {
            EditPath();
            MoveTail(Time.fixedDeltaTime*speed);
        }
    }

    private void EditPath()
    {
        if (continuous||(head.transform.position - pathList[0].pos).magnitude > pathSpacing)
            pathList.Insert(0, new MovementInfo(head.transform.position, head.transform.rotation));

        // + initLength 纯粹为了增加点长度
        if (pathList.Count > (tailList.Count+ initLength) * tailMargin / pathSpacing) 
        {
            pathList.RemoveAt(pathList.Count - 1);
        }
    }

    private void MoveTail(float s)
    {
        foreach (var body in tailList)
        {
            // head不移动
            if(body.index>0)
            {
                int pathIndex = Mathf.Clamp(Mathf.FloorToInt((body.index - 1) * tailMargin / pathSpacing), 0, pathList.Count - 1);
                body.Move(pathList[pathIndex], s, smoothDampTime);
            }
        }
    }

    public void AddTail()
    {
        TailMovment prevBody = tailList[tailList.Count-1];

        GameObject tail = Instantiate(tailPrefab, prevBody.transform.position, prevBody.transform.rotation);
        TailMovment tailMovement = tail.AddComponent<TailMovment>();

        tailMovement.Init(tailList.Count,prevBody);

        tailList.Add(tailMovement);
    }

    public void AddHead()
    {
        TailMovment tailMovement = head.AddComponent<TailMovment>();

        tailMovement.Init(tailList.Count, null);

        tailList.Add(tailMovement);
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            foreach (var pos in pathList)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(pos.pos, 0.1f);
            }
        }
    }
}

