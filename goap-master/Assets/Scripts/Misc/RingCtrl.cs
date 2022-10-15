using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingCtrl : MonoBehaviour
{
    private SpriteRenderer r;
    public LayerMask layer;
    public float radius = 3;
    public string originLayerName;
    public string enableLayerName; //开启物理判断的时候的layer

    public BoxCollider[] box;       //由slot来初始化

    void Start()
    {
        r = GetComponent<SpriteRenderer>();
        //box = GetComponentInParent<BoxCollider>();
        r.enabled = false;
    }

    public void Toggle(bool en)
    {
        r.enabled = en;
        foreach (var val in box)
        {
            if (en)
            {
                val.gameObject.layer =LayerMask.NameToLayer( enableLayerName);
            }
            else
            {
                val.gameObject.layer = LayerMask.NameToLayer( originLayerName);
            }
        }

    }

    private float GetMaximumScale()
    {
        float scale = Mathf.Max(transform.localScale.x, transform.localScale.y);
        return Mathf.Max(scale, transform.localScale.z);
    }

    Collider[] c = new Collider[10];    //就先设置10个吧
    public bool CanPlace { get; private set; }
    public bool CheckSphere()
    {
        //int num = Physics.OverlapBoxNonAlloc(box.transform.TransformPoint( box.center), box.transform.TransformDirection( box.size / 2), c, box.transform.rotation, layer);     //这个的判断有点问题
        int num = Physics.OverlapSphereNonAlloc(transform.position, radius, c, layer);
        //bool check = Physics.CheckSphere(transform.position, GetMaximumScale()/2, layer); //半径/2

        if (num > 0)
        {
            r.material.color = Color.red;
            CanPlace = false;
        }
        else
        {
            r.material.color = Color.green;

            CanPlace = true;
        }
        return CanPlace;
    }

    // Update is called once per frame
    void Update()
    {
        if (r.enabled)
        {
            CheckSphere();
        }
    }

    private void OnDrawGizmos()
    {
        //if (box)
        //{
        //    Gizmos.DrawWireCube(box.transform.TransformPoint(box.center), box.transform.TransformDirection(box.size));
        //}
        Gizmos.DrawSphere(transform.position, radius);
    }
}
