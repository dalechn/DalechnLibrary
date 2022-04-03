using System.Collections.Generic;
using UnityEngine;

public class SnakeMovment : MonoBehaviour
{
    public float speed = 5;
    public float tailSpeedRate = 1.5f;
    public float rotationSpeed = 180;
    public uint initLength = 3;
    public float tailMargin = 0.8f;
    public float pathSpacing = 0.001f;

    public GameObject head;
    public GameObject tailPrefab;

    private LinkedList<GameObject> tailList = new LinkedList<GameObject>();
    private List<Vector3> pathList = new List<Vector3>();

    private void Start()
    {
        pathList.Insert(0, head.transform.position);

        tailList.AddLast(head);
        for (int i = 0; i < initLength; i++)
        {
            AddTail();
        }
    }

    private void Update()
    {
        EditPath();
        Move();
    }

    private void EditPath()
    {
        if ((head.transform.position - pathList[0]).magnitude > pathSpacing)
            pathList.Insert(0, head.transform.position);

        if (pathList.Count > (tailList.Count+ initLength) * tailMargin / pathSpacing) 
        {
            pathList.RemoveAt(pathList.Count - 1);
        }
    }

    private void Move()
    {
        //if (Input.GetKey(KeyCode.W))
        //{
        //    head.transform.position += head.transform.forward * speed * Time.deltaTime;

        //    float steerDirection = Input.GetAxis("Horizontal");
        //    head.transform.Rotate(Vector3.up * steerDirection * rotationSpeed * Time.deltaTime);

        //    MoveTail();
        //}
        MoveTail();
    }

    private void MoveTail()
    {
        foreach (var body in tailList)
        {
            TailMovment tail = body.GetComponent<TailMovment>();
            if (tail)
            {
                int pathIndex = Mathf.Clamp(Mathf.FloorToInt((tail.index - 1) * tailMargin / pathSpacing), 0, pathList.Count - 1);
                tail.Move(pathList[pathIndex], speed * tailSpeedRate);
            }
        }
    }

    public void AddTail()
    {
        GameObject lastBody = tailList.Last.Value;

        GameObject tail = Instantiate(tailPrefab, lastBody.transform.position, lastBody.transform.rotation);
        TailMovment tailMovement = tail.GetComponent<TailMovment>();
        if (tailMovement == null)
        {
            tailMovement= tail.AddComponent<TailMovment>();
        }
        tailMovement.index = tailList.Count;
        tailList.AddLast(tail);
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            foreach (Vector3 pos in pathList)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(pos, 0.1f);
            }
        }
    }
}

