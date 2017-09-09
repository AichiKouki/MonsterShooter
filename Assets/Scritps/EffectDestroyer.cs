using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroyer : MonoBehaviour {
	private float del;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		del += Time.deltaTime;
		if (del > 3) Destroy (gameObject);
	}
}
