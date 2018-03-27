using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour {

public Projectile weapon;
public int shotCount;
public float sprayAngle;
public float delayBetweenAttacks;

private Vector3 trajectory;

	// Use this for initialization
	void Start () {
		trajectory = Vector3.right;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (shotCount > 0) {
			print (trajectory);
			trajectory = Quaternion.AngleAxis(sprayAngle, Vector3.forward) * trajectory;
			shotCount -= 1;

			Projectile bullet = Instantiate (weapon,transform.position,Quaternion.identity);
			bullet.trajectory = trajectory;
		}
	}
}
