using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class MovingPlatform : MonoBehaviour
    {
        public float leftBound = -10;
        public float rightBound = 1;
        public Direction initialDirection = Direction.Left;

        public float speed = .25f;

        public Vector3 Velocity {
            get { return _velocity; }
        }

        private Direction _currentDirection;

        private Vector3 _initialPosition;
        private Vector3 _velocity = Vector3.zero;

        void Awake()
        {
            _initialPosition = transform.position;
            _currentDirection = initialDirection;
        }

        void Update()
        {
            if (transform.position.x <= leftBound)
            {
                _currentDirection = Direction.Right;
            }
            else if (transform.position.x >= rightBound)
            {
                _currentDirection = Direction.Left;
            }

            var directionMultiplier = _currentDirection == Direction.Left ? -1 : 1;
            var appliedSpeed = speed* directionMultiplier;

            _velocity.x = appliedSpeed;
            Debug.Log(_velocity);
            transform.position += _velocity * Time.deltaTime;
        }
    }
}