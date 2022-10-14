using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform camTr;
    private Canvas canvas;
    public bool isBack = false;
    public bool onlyY;

    void Start()
    {
        camTr = Camera.main.transform;

        canvas = GetComponent<Canvas>();
        if(canvas&&!canvas.worldCamera)
        {
            canvas.worldCamera = Camera.main;
        }
    }

    private void Update()
    {
        if(onlyY)
        {
            transform.eulerAngles = new Vector3(0, camTr.rotation.eulerAngles.y+ (isBack ?180:0), 0);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(isBack ? -camTr.forward : camTr.forward, camTr.up);
        }
    }
}
