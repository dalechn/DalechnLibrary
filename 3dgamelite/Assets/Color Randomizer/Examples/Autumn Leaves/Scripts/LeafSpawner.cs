using UnityEngine;
using System.Collections;

[RequireComponent (typeof (RandomPointInBoxCollider))]
public class LeafSpawner : MonoBehaviour {

	public GameObject leafPrefab;
	public float spawnInterval = .3f;
	public int spawnThisManyPerInterval = 3; // how many leaves to spawn each spawn interval
	public float maxTorque;
	public ColorRandomizer colorRandomizer;
	public float lifeTime = 20f; // after how many seconds to deactivate

	RandomPointInBoxCollider randomPointFinder;

	Transform leafContainer;

	// Use this for initialization
	void Awake () {
		randomPointFinder = GetComponent<RandomPointInBoxCollider>();
	}

	void Start () {

		// find or create container object for leaves
		GameObject leafContainerObj = GameObject.Find("Leaf Container");
		if (leafContainerObj == null) {
			leafContainerObj = new GameObject("Leaf Container");

		}
		leafContainer = leafContainerObj.transform;

		// make sure to have a color randomizer
//		if (colorRandomizer == null) {
//			colorRandomizer = FindObjectOfType<ColorRandomizer>();
//			if (colorRandomizer == null) {
//				Debug.LogError("No Color Randomizer is assigned to the Leaf Spawner.");
//			}
//		}

		// repeatedly spawn leaves
		InvokeRepeating("SpawnLeaves", spawnInterval, spawnInterval);

		// invoke spawning stop
		Invoke ("StopSpawning", lifeTime);
	}

	// spawn a certain amount of leaves
	void SpawnLeaves () {
		for (int i = 0; i < spawnThisManyPerInterval; i++) {
			SpawnLeaf();
		}
	}

	// sawpn one leaf
	void SpawnLeaf() {


		// spawn the leaf at right position and rotation
		Vector3 spawnPos = randomPointFinder.GetPoint();		
		Quaternion randomRot = Quaternion.Euler(Random.Range(0f, 360f),
		                                             Random.Range(0f, 360f),
		                                             Random.Range(0f, 360f));
		GameObject newLeaf = (GameObject)Instantiate(leafPrefab, spawnPos, randomRot);

		// generate a random color and assign it to all renderers in the leaf
		Color randomColor = colorRandomizer.RandomColor();
		Renderer[] renderers = newLeaf.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in renderers) {
			renderer.material.color = randomColor;
		}

		// add a random torque
		Vector3 randomTorque = new Vector3(Random.Range(0f, maxTorque),
		                                   Random.Range(0f, maxTorque),
		                                   Random.Range(0f, maxTorque));
		Rigidbody rb = newLeaf.GetComponent<Rigidbody>();
		rb.AddTorque(randomTorque, ForceMode.VelocityChange);

		// parent the leaf so it looks nicer in the hierarchy
		newLeaf.GetComponent<Transform>().SetParent(leafContainer);


	}

	void StopSpawning() {
		CancelInvoke("SpawnLeaves");
		gameObject.SetActive(false);
	}

}
