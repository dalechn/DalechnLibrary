using UnityEngine;
using System.Collections;

namespace RootMotion.Demos
{

    /// <summary>
    /// User input for a third person character controller.
    /// </summary>
    public class UserControlThirdPerson : MonoBehaviour
    {

        public struct State
        {
            public int actionIndex;
            public GameObject lookObject;
            public Vector3 lookPos;

            public Vector3 move;
            public bool crouch;
            public bool jump;
            public bool strafe;
        }

        public GenericInput horizontal;
        public GenericInput vertical;
        public GenericInput jump;
        public GenericInput crouch;
        public GenericInput strafe;
        public GenericInput walk;

        public bool walkByDefault;

        public State state = new State();

        protected Transform cam;

        protected virtual void Start()
        {
            cam = Camera.main.transform;
        }

        protected virtual void Update()
        {
            MoveState();

            state.crouch = crouch.useInput && crouch.GetButton();
            state.jump = jump.useInput && jump.GetButton();
            state.strafe = state.lookObject ? true : strafe.useInput && strafe.GetButton();
        }

        protected virtual void MoveState()
        {
            Vector3 inputDir = new Vector3(horizontal.GetAxis(), 0, vertical.GetAxis());

            Vector3 move = cam.rotation * inputDir.normalized;

            if (move != Vector3.zero)
            {
                Vector3 normal = transform.up;
                Vector3.OrthoNormalize(ref normal, ref move);
                state.move = move;
            }
            else
                state.move = Vector3.zero;

            //        Vector3 move = Vector3.ProjectOnPlane(cam.transform.forward, transform.up).normalized * inputDir.z +
            //Vector3.ProjectOnPlane(cam.transform.right, transform.up).normalized * inputDir.x;
            //        if (move.magnitude > 1) move.Normalize();
            //        state.move = move;


            bool walkToggle = walk.GetButton();

            float walkMultiplier = (walkByDefault ? walkToggle ? 1 : 0.5f : walkToggle ? 0.5f : 1);
            state.move *= walkMultiplier;

            state.lookPos = state.lookObject ? state.lookObject.transform.position : transform.position + cam.forward * 100f;
        }

        //public void UpdateInput(Quaternion fromTo)
        //{
        //    state.move = fromTo * state.move;
        //}

        public virtual void SetLookObject(GameObject lookObject)
        {
            state.lookObject = lookObject;
        }
    }

}

