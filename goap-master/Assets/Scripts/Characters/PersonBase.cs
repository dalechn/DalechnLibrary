using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

	protected CharacterState previousState, currentState;

	protected virtual void Awake()
    {
		behaviorTree = GetComponent<BehaviorTree>();

		ai = GetComponent<NavMeshAgent>();

		animationHandle = GetComponentInChildren<Spine.Unity.Examples.SkeletonAnimationHandleExample>();

		tr = GetComponent<Transform>();
	}

	protected virtual void Start()
	{
	
	}


	protected virtual void Update()
	{
		Vector3 input = ai.velocity;
		if (input.magnitude == 0 )
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
