using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

public float speed = 1f;		// only used with bullets
public int damage = 1;
public Vector2 trajectory;
public float launchForce = 10f; // only used with parabolic

public bool bullet, parabolic;

private Rigidbody2D rb;

	void Start ()
	{
		if (parabolic) 
		{
			rb = GetComponent<Rigidbody2D>();
			rb.bodyType = RigidbodyType2D.Dynamic;
			rb.AddForce(trajectory * launchForce);
		}
	}

	void Update ()
	{

		if (bullet) {
			transform.Translate (trajectory * speed * Time.deltaTime);
		}


	}

	void OnBecameInvisible ()
	{
		Destroy(gameObject);	// clean up bullets that fly off-screen
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		EnemyHealth damagedEnemy = coll.GetComponent<EnemyHealth>(); //is the object an enemy that can be damaged

		if (damagedEnemy != null) 
		{
			damagedEnemy.TakeDamage (damage);
			Debug.Log(coll+" took "+damage+" damage");	
		}

		Destroy(gameObject);
	}
}
