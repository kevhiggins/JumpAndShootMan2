using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScene : MonoBehaviour {

public string targetScene; //string name of scene that this door takes you to

private LevelManager levelManager;

	void Start (){
		levelManager = FindObjectOfType<LevelManager>();
	}

	void OnTriggerEnter2D(Collider2D trigger)
	{
		if (trigger.gameObject.tag == "Player")
		{
		Debug.Log("door trigger");
		levelManager.LoadLevel(targetScene);
		}
	}

}
