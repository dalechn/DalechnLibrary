using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollower : MonoBehaviour
{
    public Transform target;
    public bool worldSpace;
    public RectTransform parentTr;

    private RectTransform tr;
    private Transform camTr;

    void Start()
    {
        tr = GetComponent<RectTransform>();

        camTr = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if(worldSpace)
        {
            tr.position = target.position;
            tr.rotation = camTr.rotation;
        }
        else
        {
            //Vector2 head = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position);
            //tr.position = head;

            Vector2 mScreenPos = Camera.main.WorldToScreenPoint(target.position);
            Vector2 mRectPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentTr, mScreenPos, null, out mRectPos);
            tr.localPosition = mRectPos ;
        }

    }
}
