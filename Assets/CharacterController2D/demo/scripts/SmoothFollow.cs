using UnityEngine;
using System.Collections;
using Prime31;


public class SmoothFollow : MonoBehaviour
{
	public Transform target;
	public float smoothDampTime = 0.2f;

	public bool useCameraBoundary; //these 4 values make up a bounding box which the camera transform will not move beyond.
	public float xMin;
	public float xMax;
	public float yMin;
	public float yMax;

	[HideInInspector]
	public new Transform transform;
	public Vector3 cameraOffset;
	public bool useFixedUpdate = false;
	
	private CharacterController2D _playerController;
	private Vector3 _smoothDampVelocity;

	private Vector3 cameraTarget;
	
	
	void Awake()
	{
		transform = gameObject.transform;
		_playerController = target.GetComponent<CharacterController2D>();
	}
	
	
	void LateUpdate()
	{
		if( !useFixedUpdate )
			updateCameraPosition();
	}


	void FixedUpdate()
	{
		if( useFixedUpdate )
			updateCameraPosition();
	}


	void updateCameraPosition ()
	{
		if (useCameraBoundary) // look at the target object's transform and store it as cameraTarget. 
		{					   // if using camera boundary, mathf.clamp will keep the cameraTarget within the defined limits before the camera starts moving toward it
			cameraTarget = new Vector3 (Mathf.Clamp (target.position.x, xMin, xMax), Mathf.Clamp (target.position.y, yMin, yMax), target.position.z);
		} else {
			cameraTarget = target.position;
		}

		if (_playerController == null) {
			transform.position = Vector3.SmoothDamp (transform.position, cameraTarget - cameraOffset, ref _smoothDampVelocity, smoothDampTime);
			return;
		}
		
		if (_playerController.velocity.x > 0) {
			transform.position = Vector3.SmoothDamp (transform.position, cameraTarget - cameraOffset, ref _smoothDampVelocity, smoothDampTime);
		} else {
			var leftOffset = cameraOffset;
			leftOffset.x *= -1;
			transform.position = Vector3.SmoothDamp (transform.position, cameraTarget - leftOffset, ref _smoothDampVelocity, smoothDampTime);
		}

	}
	
}
