using UnityEngine;
using System.Collections;

public class RemoveMeshRenderer : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		MeshRenderer renderer = GetComponent<MeshRenderer> ();
		Destroy(renderer);
	}

}
