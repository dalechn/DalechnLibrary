using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
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

	Spine.Unity.Examples.SkeletonAnimationHandleExample animationHandle;
	IAstarAI ai;

	CharacterState previousState, currentState;

	protected void Awake()
	{
		ai = GetComponent<IAstarAI>();

		animationHandle = GetComponentInChildren<Spine.Unity.Examples.SkeletonAnimationHandleExample>();
	}

	protected void Update()
	{
		Vector3 input = ai.velocity;
		if (input.x == 0 && input.y == 0)
			currentState = CharacterState.Idle;
		else
			currentState = (input.magnitude > ai.maxSpeed/2) ? CharacterState.Run : CharacterState.Walk;

		bool stateChanged = previousState != currentState;
		previousState = currentState;

		if (stateChanged)
			HandleStateChanged();

		if (input.x != 0)
			animationHandle.SetFlip(input.x);
	}


	void HandleStateChanged()
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
