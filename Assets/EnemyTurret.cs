using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class EnemyTurret : MonoBehaviour {

public Projectile weapon;
public int shotCount;	//the constant for how many bullets per volley
[SerializeField]
private	int shotMagazine;	

public float fireRate; // the constant for time delay between each shot in a volley. Null means bursting every bullet simultaneously
[SerializeField]
private float fireRateTimer; 

public float sprayAngle; // clockwise change in degrees between trajectory of each bullet in a volley. Null means no change.

public float cooldownBetweenVolley; // cooldown period between full volleys
[SerializeField]
private float cooldownTimer; 

public bool facingLeft;
[Range(-180,180)]
public int aimOffset;	// gives finer control of starting trajectory. Rotate starting trajectory additional aimOffset counterclockwise degrees, starting from a left or right vector
	[SerializeField]
private Vector3 trajectory;
	[SerializeField]
private Vector3 startingDirection;



	// Use this for initialization
	void Start ()
	{

		if (facingLeft) {
			startingDirection = Vector3.left;
		} else {
			startingDirection = Vector3.right;
		}

		startingDirection = Quaternion.AngleAxis(aimOffset, Vector3.forward) * startingDirection;

		cooldownTimer = cooldownBetweenVolley;
		fireRateTimer = fireRate;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{

		if (shotMagazine > 0) // if there's bullets left to shoot in the current volley

		{		
			if (fireRateTimer > 0) 	
			{
				fireRateTimer -= Time.deltaTime;	//decrement the fire rate timer if there's time on the clock
			} 
			else 									// if there's no time left, then it's time to shoot
			{
				//print (trajectory);
				shotMagazine -= 1;
				fireRateTimer = fireRate;

				Projectile bullet = Instantiate (weapon, transform.position, Quaternion.identity);
				bullet.trajectory = trajectory;

				trajectory = Quaternion.AngleAxis (sprayAngle, Vector3.forward) * trajectory; //rotate trajectory vector for next shot by sprayAngle using quaternion sorcery.

				if (shotMagazine <= 0) 
				{
					cooldownTimer = cooldownBetweenVolley; // reset the volley cooldown timer when the magazine is empty
				}
			}
		}

		if (cooldownTimer >= 0) 
		{
			cooldownTimer -= Time.deltaTime;

			if (cooldownTimer <= 0) 
			{
				trajectory = startingDirection; // reset to initial trajectory of gun and refill magazine
				shotMagazine = shotCount;

			}
		}

	}

	void OnDrawGizmosSelected()  // gizmo visualization of current trajectory
	{
        Gizmos.color = Color.red;
        Vector3 direction = trajectory * 3;
        Gizmos.DrawRay(transform.position, direction);
    }
}
