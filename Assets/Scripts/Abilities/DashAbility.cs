using Assets.Scripts;
using Prime31;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController2D))]
public class DashAbility : MonoBehaviour
{
    public float dashDuration = .25f;
    public float dashSpeed = 12f;
    public float dashCooldown = .5f;
    public UnityEvent onStart = new UnityEvent();

    public bool IsDashing { get; set; }

    public Vector3 Velocity
    {
        get
        {
            var directionMultiplier = transform.localScale.x > 0 ? 1 : -1;
            var x = directionMultiplier * dashSpeed;
            return new Vector3(x, 0f, 0f);
        }
    }

    private bool _hasAirDashedSinceJump;
    private bool _onCooldown = false;

    private CharacterController2D _controller;
    private Timer _timer;

    void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _timer = new Timer();
    }

    public void Try()
    {
        if (!IsDashing
            && !_hasAirDashedSinceJump
            && !_onCooldown)
        {
            IsDashing = true;
            _onCooldown = true;
            _hasAirDashedSinceJump = true;

            _timer.Countdown(dashDuration, () =>
            {
                IsDashing = false;
            });

            _timer.Countdown(dashCooldown, () =>
            {
                _onCooldown = false;
            });

            onStart.Invoke();
        }
    }

    void LateUpdate()
    {
        if (_controller.isGrounded)
        {
            _hasAirDashedSinceJump = false;
        }
    }
}
