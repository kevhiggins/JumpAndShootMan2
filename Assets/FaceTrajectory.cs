using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTrajectory : MonoBehaviour {

public Transform obj;
[SerializeField]
private Vector3 previousLocation;
	[SerializeField]
private Vector3 currentTrajectory;

	// Use this for initialization
	void Start () {
		obj = transform.root;

	}
	
	// Update is called once per frame
	void Update ()
	{

		if (obj.transform.position != previousLocation) 
		{
			currentTrajectory = obj.transform.position - previousLocation;
			transform.right = currentTrajectory;
			previousLocation = obj.transform.position;
		}

		
	}
}
