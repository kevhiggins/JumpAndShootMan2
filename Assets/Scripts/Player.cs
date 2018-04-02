using Assets.Scripts;
using Assets.Scripts.Abilities;
using Assets.Scripts.Environment;
using Prime31;
using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController2D))]
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

    private MovingPlatform _movingPlatform;
    private MovingPlatform _horizontalMovingPlatform;
    private HitDirectionResolver _hitDirectionResolver;

    public float VelocityX { get { return _velocity.x; } }
    public float VelocityY { get { return _velocity.y; } }
    public Vector3 ExternalVelocity
    {
        get
        {
            return _horizontalMovingPlatform != null || _movingPlatform != null ? _horizontalMovingPlatform.Velocity : Vector3.zero;
        }
    }

    public bool IsGrounded { get { return _controller.isGrounded || OnMovingPlatform; } }
    public bool OnMovingPlatform { get { return _movingPlatform != null; } }

    void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _dashAbility = GetComponent<DashAbility>();
        _horizontalMoveAbility = GetComponent<HorizontalMoveAbility>();

        // listen to some events for illustration purposes
        //_controller.onControllerCollidedEvent += onControllerCollider;
        //_controller.onTriggerEnterEvent += onTriggerEnterEvent;
        //_controller.onTriggerExitEvent += onTriggerExitEvent;
        _controller.onTriggerStayEvent += onTriggerStayEvent;
        _hitDirectionResolver = new HitDirectionResolver();

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

    void onTriggerStayEvent(Collider2D col)
    {
        var movingPlatform = col.GetComponent<MovingPlatform>();
        if (movingPlatform == null)
        {
            return;
        }

        var direction = _hitDirectionResolver.FindDirection(gameObject, col, _controller.platformMask);



        //var platformCollider = movingPlatform.GetComponent<BoxCollider2D>();
        //var playerCollider = GetComponent<BoxCollider2D>();
        //var distance2D = playerCollider.Distance(platformCollider);


        if (_movingPlatform == null && direction == HitDirection.Top)
        {
            _movingPlatform = movingPlatform;
            _controller.warpToGrounded();

            //_movingPlatform.SetCollision(false);
        }

        if (_horizontalMovingPlatform == null &&
            (
                (direction == HitDirection.Left && movingPlatform.HorizontalDirection == HorizontalDirection.Left)
             || (direction == HitDirection.Right && movingPlatform.HorizontalDirection == HorizontalDirection.Right)
             || direction == HitDirection.Top
             ))
        {
            _horizontalMovingPlatform = movingPlatform;
            //movingPlatform.SetCollision(false);
        }
        //else if (_movingPlatform != null && _movingPlatform.GetInstanceID() == movingPlatform.GetInstanceID())
        //{
        //    _movingPlatform = null;
        //}
    }

    //void onTriggerStayEvent(Collider2D col)
    //{
    //    var movingPlatform = col.GetComponent<MovingPlatform>();
    //    if (movingPlatform == null)
    //    {
    //        return;
    //    }

    //    var platformCollider = movingPlatform.GetComponent<BoxCollider2D>();
    //    var playerCollider = GetComponent<BoxCollider2D>();
    //    var distance2D = playerCollider.Distance(platformCollider);

    //    if (distance2D.normal == Vector2.down)
    //    {
    //        _movingPlatform = movingPlatform;
    //    }
    //    else if (_movingPlatform != null &&_movingPlatform.GetInstanceID() == movingPlatform.GetInstanceID())
    //    {
    //        _movingPlatform = null;
    //    }
    //}

    #endregion

    // TODO fix being able to run into moving platforms to pass through
    // TODO fix dash animation on moving platforms

    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void LateUpdate()
    {
        //if (_horizontalMovingPlatform != null)
        //{
        //    _horizontalMovingPlatform.SetCollision(false);
        //}


        if (IsGrounded)
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
        if (IsGrounded && Input.GetButtonDown("Jump"))
        {
            _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            OnJump.Invoke();
        }

        if (_horizontalMovingPlatform != null)
        {
            var platformCollider = _horizontalMovingPlatform.GetComponent<BoxCollider2D>();
            var playerCollider = GetComponent<BoxCollider2D>();

            var platformLeft = platformCollider.bounds.min;
            var platformRight = platformCollider.bounds.max;

            var playerLeft = playerCollider.bounds.min;
            var playerRight = playerCollider.bounds.max;

            if (playerRight.x <= platformLeft.x || playerLeft.x >= platformRight.x || Input.GetButtonDown("Jump"))
            {
                //_horizontalMovingPlatform.SetCollision(true);
                _movingPlatform = null;
                _horizontalMovingPlatform = null;
                //_velocity.y += ExternalVelocity.y;
                _velocity += ExternalVelocity;
            }
        }

        //if (OnMovingPlatform)
        //{
        //    var platformCollider = _movingPlatform.GetComponent<BoxCollider2D>();
        //    var playerCollider = GetComponent<BoxCollider2D>();
        //    var distance2D = playerCollider.Distance(platformCollider);
        //    transform.position = new Vector3(transform.position.x, transform.position.y - distance2D.distance,
        //        transform.position.z);
        //    _velocity.y = ExternalVelocity.y;
        //    //_controller.warpToGrounded();
        //}

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
            if (!OnMovingPlatform)
            {
                _velocity.y += gravity * Time.deltaTime;
            }
        }

        if (OnMovingPlatform)
        {
            //var platformCollider = _movingPlatform.GetComponent<BoxCollider2D>();
            //var playerCollider = GetComponent<BoxCollider2D>();
            //var distance2D = playerCollider.Distance(platformCollider);
            //var distance = playerCollider.bounds.min.y - platformCollider.bounds.max.y;

            transform.position = new Vector3(transform.position.x, transform.position.y + ExternalVelocity.y * Time.deltaTime, transform.position.z);

            //_velocity.y = ExternalVelocity.y;
            //_controller.warpToGrounded();
        }


        //		 if holding down bump up our movement amount and turn off one way platform detection for a frame.
        //		 this lets us jump down through one way platforms
        //if (IsGrounded && Input.GetKey(KeyCode.DownArrow))
        //{
        //    _velocity.y *= 3f;
        //    _controller.ignoreOneWayPlatformsThisFrame = true;
        //}

        //if (!IsGrounded && !Input.GetButton("Jump") && _velocity.y >= airBreakThreshold)
        //{
        //    _velocity.y = airBreakSpeed;
        //}



        _controller.move(_velocity * Time.deltaTime);



        // grab our current _velocity to use as a base for all calculations
        _velocity = _controller.velocity;
        _hitDirectionResolver.SavePosition(transform.position);
    }

}
