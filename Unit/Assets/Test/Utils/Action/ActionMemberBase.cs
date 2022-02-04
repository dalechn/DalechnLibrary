using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CJTools
{
    public class ActionMemberBase
    {
        public Vector3 originVector;
        public Vector3 destVector;

        public ActionMemberBase(Vector3 originVector, Vector3 destVector)
        {
            this.originVector = originVector;
            this.destVector = destVector;
        }

        public virtual void RunAction(GameObject obj, float t)
        {

        }
    }

    public class ActionMemberMove : ActionMemberBase
    {
        public ActionMemberMove(Vector3 originVector, Vector3 destVector) : base(originVector, destVector)
        {

        }

        public override void RunAction(GameObject obj, float t)
        {
            obj.transform.position = Vector3.Lerp(originVector, destVector, t);
        }
    }

    public class ActionMemberJump : ActionMemberBase
    {
        float height = 2;

        public ActionMemberJump(Vector3 originVector, Vector3 destVector) : base(originVector, destVector)
        {

        }

        public override void RunAction(GameObject obj, float t)
        {
            obj.transform.position = Vector3.Lerp(originVector, destVector, t) + Vector3.up * height * Mathf.Sin(t * Mathf.PI);
        }
    }

    public class ActionMemberScale : ActionMemberBase
    {
        public ActionMemberScale(Vector3 originVector, Vector3 destVector) : base(originVector, destVector)
        {

        }

        public override void RunAction(GameObject obj, float t)
        {
            obj.transform.localScale = Vector3.Lerp(originVector, destVector, t);
        }
    }

    public class ActionMemberAngle : ActionMemberBase
    {
        public ActionMemberAngle(Vector3 originVector, Vector3 destVector) : base(originVector, destVector)
        {

        }

        public override void RunAction(GameObject obj, float t)
        {
            obj.transform.localEulerAngles = Vector3.Lerp(originVector, destVector, t);
        }
    }

    public class ActionMemberQuaternion : ActionMemberBase
    {
        public Quaternion originQuaternion;
        public Quaternion destQuaternion;

        public ActionMemberQuaternion(Quaternion originQuaternion, Quaternion destQuaternion) : base(Vector3.zero, Vector3.zero)
        {
            this.originQuaternion = originQuaternion;
            this.destQuaternion = destQuaternion;
        }

        public override void RunAction(GameObject obj, float t)
        {
            obj.transform.rotation = Quaternion.Lerp(originQuaternion, destQuaternion, t);
        }
    }


}
