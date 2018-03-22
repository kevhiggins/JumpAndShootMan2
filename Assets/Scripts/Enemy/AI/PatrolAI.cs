using System;
using Assets.Scripts.Abilities;
using Prime31;
using UnityEngine;

namespace Assets.Scripts.Enemy.AI
{
    [RequireComponent(typeof(HorizontalMoveAbility), typeof(CharacterController2D))]
    public class PatrolAI : MonoBehaviour, IUnitVelocity
    {
        enum Direction
        {
            Left,
            Right
        }

        public float VelocityX { get { return _velocity.x; } }
        public float VelocityY { get { return _velocity.y; } }

        public float gravity = -25f;

        private HorizontalMoveAbility _horizontalMoveAbility;
        private CharacterController2D _controller;

        private Direction _direction = Direction.Left;
        private Vector3 _velocity;
        private float _lastDistance;

        void Awake()
        {
            _horizontalMoveAbility = GetComponent<HorizontalMoveAbility>();
            _controller = GetComponent<CharacterController2D>();
        }

        void Update()
        {
            if (_controller.isGrounded)
            {
                _velocity.y = 0;
            }

            _velocity.y += gravity * Time.deltaTime;

            if (_controller.isGrounded)
            {
                if (_direction == Direction.Left)
                {
                    _horizontalMoveAbility.TryLeft();
                }
                else
                {
                    _horizontalMoveAbility.TryRight();
                }

                _velocity.x = _horizontalMoveAbility.Speed;

                var moveVector = _velocity * Time.deltaTime;
                var predictedPosition = transform.position + moveVector;
                var origin = new Vector2(predictedPosition.x, predictedPosition.y);

                var result = Physics2D.Raycast(origin, Vector2.down, 1, _controller.platformMask);

                if (result.collider == null)
                {
                    SwitchDirection();
                    _velocity.x = 0;
                }
                _lastDistance = result.distance;
            }

            _controller.move(_velocity * Time.deltaTime);

            
        }

        private void SwitchDirection()
        {
            _direction = _direction == Direction.Left ? Direction.Right : Direction.Left;
        }
    }
}
