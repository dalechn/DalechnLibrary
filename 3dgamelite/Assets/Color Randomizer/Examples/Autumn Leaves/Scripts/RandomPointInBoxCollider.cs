using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider))]
public class RandomPointInBoxCollider : MonoBehaviour {

	BoxCollider boxCollider;

	// Use this for initialization
	void Awake () {
		boxCollider = GetComponent<BoxCollider>();
	}

	// returns a random point within a box collider
	public Vector3 GetPoint() {
		Vector3 min = boxCollider.bounds.min;
		Vector3 max = boxCollider.bounds.max;
		return new Vector3(Random.Range(min.x, max.x),
		                   Random.Range(min.y, max.y),
		                   Random.Range(min.z, max.z));

	}
}
