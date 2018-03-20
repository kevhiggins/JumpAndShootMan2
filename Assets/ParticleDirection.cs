using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDirection : MonoBehaviour {

private Transform parentTransform;

	// Use this for initialization
	void Start () {
		parentTransform = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {

		var facing = parentTransform.localScale.x;
		transform.localScale = new Vector3(facing, transform.localScale.y, transform.localScale.z);
	}
}
