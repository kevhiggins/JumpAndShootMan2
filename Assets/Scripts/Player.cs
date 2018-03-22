using Assets.Scripts;
using Assets.Scripts.Abilities;
using Prime31;
using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DashAbility))]
[RequireComponent(typeof(HorizontalMoveAbility))]
public class Player : MonoBehaviour, IUnitVelocity
{
    // movement config
    public float gravity = -25f;
    public float airBreakThreshold = 6f; //when jump is released, air breaking will kick in if vspeed is higher than this
    public float airBreakSpeed = 3f;	//when airbreak happens, vspeed is reduced to this
    public float jumpHeight = 3f;

    public UnityEvent OnJump = new UnityEvent();

    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;

    private DashAbility _dashAbility;
    private HorizontalMoveAbility _horizontalMoveAbility;

    public float VelocityX { get { return _velocity.x; } }
    public float VelocityY { get { return _velocity.y; } }

    void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _dashAbility = GetComponent<DashAbility>();
        _horizontalMoveAbility = GetComponent<HorizontalMoveAbility>();

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

    void onControllerCollider(RaycastHit2D hit)
    {
        // bail out on plain old ground hits cause they arent very interesting
        if (hit.normal.y == 1f)
            return;

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }


    void onTriggerEnterEvent(Collider2D col)
    {
        Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
    }


    void onTriggerExitEvent(Collider2D col)
    {
        Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
    }

    #endregion


    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update()
    {
        if (_controller.isGrounded)
        {
            _velocity.y = 0;
        }

        if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            _horizontalMoveAbility.TryRight();
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            _horizontalMoveAbility.TryLeft();
        }
        else
        {
            _horizontalMoveAbility.TryIdle();
        }

        // we can only jump whilst grounded
        if (_controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            OnJump.Invoke();
        }

        if (Input.GetButtonDown("Dash"))
        {
            _dashAbility.Try();
        }

        if (_dashAbility.IsDashing)
        {
            _velocity = _dashAbility.Velocity;
        }
        else
        {
            _velocity.x = _horizontalMoveAbility.Speed;
            // apply gravity before moving
            _velocity.y += gravity * Time.deltaTime;
        }

        //		 if holding down bump up our movement amount and turn off one way platform detection for a frame.
        //		 this lets us jump down through one way platforms
        if (_controller.isGrounded && Input.GetKey(KeyCode.DownArrow))
        {
            _velocity.y *= 3f;
            _controller.ignoreOneWayPlatformsThisFrame = true;
        }

        if (!Input.GetButton("Jump") && _velocity.y >= airBreakThreshold)
        {
            _velocity.y = airBreakSpeed;
        }



        _controller.move(_velocity * Time.deltaTime);

        // grab our current _velocity to use as a base for all calculations
        _velocity = _controller.velocity;
    }

}
