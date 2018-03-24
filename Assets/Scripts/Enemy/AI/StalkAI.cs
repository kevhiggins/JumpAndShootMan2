using Assets.Scripts.Abilities;
using Prime31;
using UnityEngine;

namespace Assets.Scripts.Enemy.AI
{
    [RequireComponent(typeof(HorizontalMoveAbility), typeof(CharacterController2D))]
    public class StalkAI : MonoBehaviour, IUnitVelocity
    {
        public float VelocityX { get { return _velocity.x; } }
        public float VelocityY { get { return _velocity.y; } }

        public float gravity = -25f;
        public LayerMask searchTargetLayers;

        private HorizontalMoveAbility _horizontalMoveAbility;
        private CharacterController2D _controller;

        private Vector3 _velocity;

        private Collider2D _platformCollider;

        private Vector3? _targetPosition;

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

            if (_platformCollider == null && _controller.isGrounded)
            {
                var hit = Physics2D.Raycast(transform.position, Vector2.down, 1, _controller.platformMask);
                _platformCollider = hit.collider;
            }

            if (_platformCollider != null)
            {
                var left = new Vector2(_platformCollider.bounds.min.x, transform.position.y);
                var right = new Vector2(_platformCollider.bounds.max.x, transform.position.y);
                var playerHit = Physics2D.Linecast(left, right, searchTargetLayers);
                if (playerHit.collider != null)
                {
                    _targetPosition = playerHit.collider.transform.position;
                    // CHASE HIM DOWN

                }

                if (_targetPosition.HasValue)
                {
                    Chase(_targetPosition.Value, _platformCollider.bounds);
                }
                else
                {
                    _velocity.x = 0;
                }
            }

            _controller.move(_velocity * Time.deltaTime);
        }

        private void Chase(Vector3 targetPosition, Bounds bounds)
        {
            var aiPosition = transform.position;
            var isTargetLeft = targetPosition.x < aiPosition.x;
            if (isTargetLeft)
            {
                _horizontalMoveAbility.TryLeft();
            }
            else
            {
                _horizontalMoveAbility.TryRight();
            }

            _velocity.x = _horizontalMoveAbility.Speed;

            var moveVector = _velocity * Time.deltaTime;
            var nextPosition = aiPosition + moveVector;

            if (isTargetLeft && nextPosition.x <= targetPosition.x
                || !isTargetLeft && nextPosition.x >= targetPosition.x)
            {
                _targetPosition = null;
                _velocity.x = 0;
            }
        }
    }
}
