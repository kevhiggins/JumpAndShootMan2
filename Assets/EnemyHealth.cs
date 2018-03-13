using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

public int health = 1;
public float deathLingerTime = 1f;
private Animator anim;


	// Use this for initialization
	void Start () {
		anim = GetComponentInParent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TakeDamage (int dmg)
	{
		health -= dmg;

		if (health <= 0) 
		{
			Death();
			GetComponent<Collider2D>().enabled = false;
			return;
		}

		if (anim != null) 
		{
			anim.Play("Hit");
		}
	}

	void Death ()
	{
		if (anim != null) 
		{
			anim.Play("Death");
			Destroy (gameObject, deathLingerTime);

		} else {Destroy (gameObject);}
	}
}
