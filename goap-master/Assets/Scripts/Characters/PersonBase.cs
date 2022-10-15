using BehaviorDesigner.Runtime;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MyShop
{

    [System.Serializable]
    public class SharedPersonBase : SharedVariable<PersonBase>
    {
        public static implicit operator SharedPersonBase(PersonBase
       value)
        { return new SharedPersonBase { Value = value }; }
    }


    public class PersonBase : MonoBehaviour
    {
        public enum CharacterState
        {
            None,
            Idle,
            Walk,
            Run,
            Crouch,
            Rise,
            Fall,
            Attack
        }

        public float moveSpeed = 5;
        internal Transform tr;

        protected BehaviorTree behaviorTree;
        protected Spine.Unity.Examples.SkeletonAnimationHandleExample animationHandle;
        protected NavMeshAgent ai;
        protected SkeletonRendererCustomMaterials customMaterials;

        protected SharedPersonBase sharedPersonBase = new SharedPersonBase();           //为行为树添加引用
        public MeshRenderer mesh { get; protected set; }

        protected CharacterState previousState, currentState;

        protected virtual void Awake()
        {
            ai = GetComponent<NavMeshAgent>();
            tr = GetComponent<Transform>();

            animationHandle = GetComponentInChildren<Spine.Unity.Examples.SkeletonAnimationHandleExample>();
            customMaterials = GetComponentInChildren<SkeletonRendererCustomMaterials>();
            mesh = animationHandle.skeletonAnimation.GetComponent<MeshRenderer>();

            behaviorTree = GetComponent<BehaviorTree>();
        }

        protected virtual void Start()
        {
            if (customMaterials)
            {
                customMaterials.enabled = false;
            }

            sharedPersonBase.Value = this;
            behaviorTree.SetVariableValue(GlobalConfig.SharedPerson, sharedPersonBase);
        }

        public void ToggleOutline(bool en)
        {
            if (customMaterials)
            {
                customMaterials.enabled = en;
            }
        }

        public void TogglePerson(bool en)
        {
            mesh.enabled = en;
        }

        protected virtual void Update()
        {
            Vector3 input = ai.velocity;
            if (input.magnitude == 0)
                currentState = CharacterState.Idle;
            else
                currentState = (input.magnitude > ai.speed / 2) ? CharacterState.Run : CharacterState.Walk;

            bool stateChanged = previousState != currentState;
            previousState = currentState;

            if (stateChanged)
                HandleStateChanged();

            if (input.x != 0)
                animationHandle.SetFlip(input.x);
        }


        protected virtual void HandleStateChanged()
        {
            string stateName = null;
            switch (currentState)
            {
                case CharacterState.Idle:
                    stateName = "idle";
                    break;
                case CharacterState.Walk:
                    stateName = "walk";
                    break;
                case CharacterState.Run:
                    stateName = "run";
                    break;
                case CharacterState.Crouch:
                    stateName = "crouch";
                    break;
                case CharacterState.Rise:
                    stateName = "rise";
                    break;
                case CharacterState.Fall:
                    stateName = "fall";
                    break;
                case CharacterState.Attack:
                    stateName = "attack";
                    break;
                default:
                    break;
            }

            animationHandle.PlayAnimationForState(stateName, 0);
        }
    }
}