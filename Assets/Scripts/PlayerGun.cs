using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour {

public Projectile weapon;
public float bulletSpeed = 1f;
public int bulletDamage = 1;

private Transform playerTransform;
private Vector2 direction;



	void Start ()
	{
		playerTransform = transform.parent.GetComponent<Transform>();

	}

	void Update ()
	{

		if (Input.GetButtonDown ("Fire1")) 
		{
			Shoot();
		}
		
	}

	void Shoot()
	{
		if (playerTransform.localScale.x > 0f) //check facing of player to know which direction bullet should travel
		{
			direction = Vector2.right;		
		} else {
			direction = Vector2.left;
		}

		Projectile bullet = Instantiate(weapon,transform.position,Quaternion.identity); //instantiate bullet and initialize with settings configured on the gun
		bullet.trajectory = direction;
		bullet.damage = bulletDamage;
		bullet.speed = bulletSpeed;
	}
}
