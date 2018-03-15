using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedPlatform : MonoBehaviour {

public float standTimeSeconds = 1f; //seconds until the platform breaks
public float resetTimeSeconds = 3f;	//seconds unti the platform returns to position.

private float standClock;	//timers
private float resetClock;

private bool ticking;	//has the platform been touched by the player and started counting down
private bool broken;	//is the platform currently broken


private Collider2D coll;
private SpriteRenderer sprite;
private Color startingColor;


	// Use this for initialization
	void Start () 
	{
		standClock = standTimeSeconds;
		resetClock = resetTimeSeconds;

		coll = GetComponent<Collider2D>();
		sprite = GetComponent<SpriteRenderer>();
		startingColor = sprite.color;
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (broken) 
		{
			resetClock -= Time.deltaTime;
			if (resetClock <= 0f) {
				Reset ();
			}
		}

		if (ticking) 
		{
			standClock -= Time.deltaTime;
			if (standClock <= 0f)
			{
				BreakPlatform();
			}

			sprite.color = Color.Lerp (Color.white, new Vector4 (0.4f,0,0,1), (1 - standClock/standTimeSeconds));
		}
		
	}

	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.gameObject.tag == "Player")
		{
			//Debug.Log("player touched block");
			ticking = true;
		}
	}


	void BreakPlatform()
	{
		//Debug.Log("breaking");
		coll.enabled = false;
		sprite.enabled = false;
		broken = true;
		ticking = false;
	}

	void Reset()
	{
		//Debug.Log("resetting");
		coll.enabled = true;
		sprite.enabled = true;
		sprite.color = startingColor;

		broken = false;
		resetClock = resetTimeSeconds;
		standClock = standTimeSeconds;
	}




}
