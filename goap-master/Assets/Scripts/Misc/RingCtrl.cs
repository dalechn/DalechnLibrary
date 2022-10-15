using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingCtrl : MonoBehaviour
{
    private SpriteRenderer r;
    public LayerMask layer;
    public float radius = 3;

    public BoxCollider box;

    void Start()
    {
        r = GetComponent<SpriteRenderer>();
        //box = GetComponentInParent<BoxCollider>();
    }

    public void Toggle(bool en)
    {
        r.enabled = en;
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
        int num = Physics.OverlapSphereNonAlloc(transform.position, radius, c, layer); //半径/2
        //bool check = Physics.CheckSphere(transform.position, GetMaximumScale(), layer);
        if (num > 0 && !(num == 1 && c[0] == box))
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
        if (box)
        {
            Gizmos.DrawWireCube(box.transform.TransformPoint(box.center), box.transform.TransformDirection(box.size));
        }
        Gizmos.DrawSphere(transform.position, radius);
    }
}
