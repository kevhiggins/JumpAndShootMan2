using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEffect : MonoBehaviour {

public ParticleSystem ps;
public Transform origin;



	void OnTriggerEnter2D(Collider2D trigger)
	{
		if (trigger.gameObject.tag == "Player")
		{
		Debug.Log("effect trigger");
		//ParticleSystem effect = Instantiate(ps,origin.position,ps.transform.rotation);
		}
	}

}
