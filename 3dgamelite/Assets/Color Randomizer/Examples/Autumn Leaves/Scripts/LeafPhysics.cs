using UnityEngine;
using System.Collections;

/// <summary>
/// A fast and simple approximation of leaf physics for demo purposes.
/// </summary>

public class LeafPhysics : MonoBehaviour {

	Transform myTransform;
	Rigidbody myRigidbody;

	public float forceFactor = .5f;
	public float updateForceInterval = 1f;
	Vector3 glideForce;

	// Use this for initialization
	void Awake () {
		myTransform = GetComponent<Transform>();
		myRigidbody = GetComponent<Rigidbody>();
		glideForce = Vector3.zero;
		InvokeRepeating("UpdateGlideForce", 0f, updateForceInterval);
	}

	// continously apply the glide force
	void FixedUpdate () {
		myRigidbody.AddForce(glideForce, ForceMode.Acceleration);
	}

	// recalculates the force applied to the leaf to simulate gliding
	void UpdateGlideForce () {
		// calculate differences between angles for "up" and current angles
		Quaternion fromTo = Quaternion.FromToRotation(myTransform.up, Vector3.up);
		Vector3 rotVector = fromTo.eulerAngles;
		float xDiff = rotVector.x;
		float zDiff = rotVector.z;

		// calculate force direction values from those difference angles
		float xDir = -DirValue (zDiff);
		float zDir = DirValue (xDiff);

		// "gravity" or downwards force is less the more gliding / sidewards force there is
		float yDir = -(1 - ((zDir + xDir) / 2f));

		glideForce = new Vector3(xDir, yDir, zDir);
//		Debug.Log ("Glide Force: " + glideForce);
		glideForce *= forceFactor;
	}

	// calculate a value for a directional force from a rotation angle to simulate gliding on air
	float DirValue(float angle) {
		if (angle > 180f) angle -= 180f; // assume symmetrical object

		if (angle > 90f) { // turned towards one side
			angle -= 90f;
			return angle / 90f;
		}
		else { // turned towards the other side
			return (90f - angle) / -90f;
		}
	}
}
