using System;
using UnityEngine;
using Prime31;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(Animator))]
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

    private float? _dashTimeAccumulation;
    private bool _hasAirDashedSinceJump;
    private float? _timeSinceDashStart;

    private CharacterController2D _controller;
    private Animator _animator;

    void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _animator = GetComponent<Animator>();
    }

    public void Try()
    {
        // TODO use a timer for the cooldown and duration
        if (!IsDashing
            && _hasAirDashedSinceJump == false
            && !_timeSinceDashStart.HasValue)
        {
            IsDashing = true;
            _hasAirDashedSinceJump = true;
            _timeSinceDashStart = 0f;
            _dashTimeAccumulation = 0f;

            onStart.Invoke();
        }
    }

    void LateUpdate()
    {
        if (_controller.isGrounded)
        {
            _hasAirDashedSinceJump = false;
        }

        if (_dashTimeAccumulation.HasValue)
        {
            _dashTimeAccumulation += Time.deltaTime;
            if (_dashTimeAccumulation >= dashDuration)
            {
                IsDashing = false;
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
    }
}
