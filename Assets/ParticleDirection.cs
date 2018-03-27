using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDirection : MonoBehaviour {

public Transform playerTransform;


	// Update is called once per frame
	void Update () {

		var facing = playerTransform.localScale.x;
		transform.localScale = new Vector3(facing, transform.localScale.y, transform.localScale.z);
	}
}
