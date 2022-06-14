using UnityEngine;
using System.Collections;

/// <summary>
/// Periodically changes the start color of the particle system using a color randomizer
/// </summary>

public class ColorUpdater : MonoBehaviour {

	public float updateInterval;
	public ColorRandomizer colorRandomizer;
	public ParticleSystem particles;


	void Start () {
		InvokeRepeating ("ChangeColor", updateInterval, updateInterval); 
	}

	// this method assigns a new randomized color to the particle system
	void ChangeColor () {
		Color c = colorRandomizer.RandomColor();
		particles.startColor = c;
	}
}
