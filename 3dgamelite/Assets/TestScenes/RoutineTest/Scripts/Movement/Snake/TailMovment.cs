using UnityEngine;
using System.Collections.Generic;

public class TailMovment : MonoBehaviour
{
    public int index;
    private Vector3 vec;
    public TailMovment prev;
    public Transform tr;

    private void Start()
    {
        tr = GetComponent<Transform>();
    }

    public void Init(int index,TailMovment prev)
    {
        this.index = index;
        this.prev = prev;
    }

    public MovementInfo GetMoveInfo()
    {
        return new MovementInfo(tr.position,tr.rotation);
    }

    public void Move(MovementInfo target ,float speed, float smoothDampTime)
    {
        //tr.rotation = Quaternion.Slerp(tr.rotation, target.rot, speed);
        tr.rotation = target.rot;
        //tr.LookAt(prev.tr.position);

        tr.position = Vector3.SmoothDamp(tr.position, target.pos, ref vec, smoothDampTime);
        tr.position = Vector3.Lerp(tr.position, target.pos, speed);
        //tr.position = Vector3.MoveTowards(tr.position, target.pos,  speed);
        //tr.position = target.pos;

    }

}
