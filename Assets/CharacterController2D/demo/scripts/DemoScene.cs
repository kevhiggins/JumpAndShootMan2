using UnityEngine;
using System.Collections;
using Prime31;
using System;


public class DemoScene : MonoBehaviour
{
	// movement config
	public float gravity = -25f;
	public float airBreakThreshold = 6f; //when jump is released, air breaking will kick in if vspeed is higher than this
	public float airBreakSpeed = 3f;	//when airbreak happens, vspeed is reduced to this
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;

    public float dashDuration = .75f;
    public float dashSpeed = 15f;
    public float dashCooldown = 1f;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;

	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;

    private bool _lastFrameWasAirDash = false;
    private bool _isDashing = false;
    private float? _dashTimeAccumulation;
    private bool _hasAirDashedSinceJump = false;
    private float? _timeSinceDashStart;
    

    //private bool _wasAirborne = false;

    
    

	void Awake ()
	{
		_animator = GetComponent<Animator> ();
		_controller = GetComponent<CharacterController2D> ();

		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;

		if (airBreakThreshold <= airBreakSpeed) 
		{
			throw new Exception("Airbreak Threshold must be higher than airbreak speed");
		}
	}


	#region Event Listeners

	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void onTriggerEnterEvent( Collider2D col )
	{
		Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
	}


	void onTriggerExitEvent( Collider2D col )
	{
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

	#endregion


	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update ()
	{
	    if (_controller.isGrounded)
	    {
	        _velocity.y = 0;
	        _hasAirDashedSinceJump = false;
	    }

		if (Input.GetAxisRaw ("Horizontal") > 0f) {
			normalizedHorizontalSpeed = 1;
			if (transform.localScale.x < 0f)
				transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);

			if (_controller.isGrounded)
				_animator.Play (Animator.StringToHash ("Run"));
		} else if (Input.GetAxisRaw ("Horizontal") < 0f) {
			normalizedHorizontalSpeed = -1;
			if (transform.localScale.x > 0f)
				transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);

			if (_controller.isGrounded)
				_animator.Play (Animator.StringToHash ("Run"));
		} else {
			normalizedHorizontalSpeed = 0;

			if (_controller.isGrounded)
				_animator.Play (Animator.StringToHash ("Idle"));
		}

		// we can only jump whilst grounded
		if (_controller.isGrounded && Input.GetButtonDown ("Jump")) {
			_velocity.y = Mathf.Sqrt (2f * jumpHeight * -gravity);
			_animator.Play (Animator.StringToHash ("Jump"));
		}
		//
	    if (_dashTimeAccumulation.HasValue)
	    {
	        _dashTimeAccumulation += Time.deltaTime;
	        if (_dashTimeAccumulation >= dashDuration)
	        {
	            _isDashing = false;
	        }
	    }

	    if (_timeSinceDashStart.HasValue)
	    {
	        _timeSinceDashStart += Time.deltaTime;
	        if (_timeSinceDashStart >= dashCooldown)
	        {
	            _timeSinceDashStart = null;
	        }
	    }

        if (Input.GetButtonDown("Dash") 
            && _lastFrameWasAirDash == false 
            && _hasAirDashedSinceJump == false
            && !_timeSinceDashStart.HasValue)
	    {
	        _lastFrameWasAirDash = true;
	        _hasAirDashedSinceJump = true;
	        _isDashing = true;
	        _timeSinceDashStart = 0f;
            
	        _dashTimeAccumulation = 0f;

			_animator.Play (Animator.StringToHash ("Dash"));
	    }
	    else
	    {
	        _lastFrameWasAirDash = false;
	    }

	    if (_isDashing)
	    {
	        var directionMultiplier = transform.localScale.x > 0 ? 1 : -1;
            _velocity.x = directionMultiplier * dashSpeed;
            _velocity.y = 0f;
	    }
	    else
	    {
	        // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
	        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
	        _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);
	        // apply gravity before moving
	        _velocity.y += gravity * Time.deltaTime;
        }



//		 if holding down bump up our movement amount and turn off one way platform detection for a frame.
//		 this lets us jump down through one way platforms
		if (_controller.isGrounded && Input.GetKey (KeyCode.DownArrow)) {
			_velocity.y *= 3f;
			_controller.ignoreOneWayPlatformsThisFrame = true;
		}

		if (!Input.GetButton ("Jump") && _velocity.y >= airBreakThreshold) {
			_velocity.y = airBreakSpeed;
		}



        _controller.move( _velocity * Time.deltaTime );

		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
	}

}
