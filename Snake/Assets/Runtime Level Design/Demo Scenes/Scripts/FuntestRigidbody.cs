using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Invector.vClassHeader("FuntestRigidbody")]
public class FuntestRigidbody : vMonoBehaviour
{
    [Invector.vEditorToolbar("Test")]
    public float t;
    [Invector.vEditorToolbar("Test2")]
    public float t2;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //if (rb)
        //{
        //    rb.MovePosition(tempPos); // 会进行插值运算
        //    //rb.position = tempPos; //快于 transform.position
        //}
    }

    void Update()
    {

    }
}
