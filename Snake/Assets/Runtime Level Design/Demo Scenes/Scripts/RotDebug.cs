using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotDebug : MonoBehaviour
{
    public Transform target;
    public Transform YPointer;

    void Start()
    {
    }

    void Update()
    {
        transform.position = target.position;
        YPointer.rotation = target.rotation;
    }

    void BroadcastMessageTest()
    {
        SendMessageUpwards("SendMessageUpwardsTest");
        Debug.Log("RotDebug BroadcastMessageTest");
    }
}
