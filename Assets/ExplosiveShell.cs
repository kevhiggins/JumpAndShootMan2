using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveShell : MonoBehaviour {

public Vector2 trajectory;
public float force;

private Rigidbody2D rb;

	// Use this for initialization
	void Start () {

	rb = GetComponent<Rigidbody2D>();

	rb.AddForce(trajectory * force);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
