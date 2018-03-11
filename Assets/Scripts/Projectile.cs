using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

public float speed = 1f;
public Vector2 trajectory;


	void Update () {

		transform.Translate(trajectory * speed * Time.deltaTime);
		
	}

	void OnBecameInvisible ()
	{
		Destroy(gameObject);
	}
}
