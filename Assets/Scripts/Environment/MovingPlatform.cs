using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class MovingPlatform : MonoBehaviour
    {
        public float leftBound = 0f;
        public float rightBound = 0f;
        public float topBound = 0f;
        public float bottomBound = 0f;

        public HorizontalDirection initialXDirection = HorizontalDirection.Left;
        public VerticalDirection initialYDirection = VerticalDirection.Up;

        public HorizontalDirection HorizontalDirection { get; private set; }

        public float xSpeed = 0f;
        public float ySpeed = 0f;

        public Vector3 Velocity
        {
            get { return _velocity; }
        }

        private VerticalDirection _currentYDirection;

        private Vector3 _initialPosition;
        private Vector3 _velocity = Vector3.zero;

        void Awake()
        {
            _initialPosition = transform.position;
            HorizontalDirection = initialXDirection;
            _currentYDirection = initialYDirection;
        }

        void Update()
        {
            CheckHorizontalDirection();
            CheckVerticalDirection();

            _velocity.x = HorizontalSpeed();
            _velocity.y = VerticalSpeed();

            transform.position += _velocity * Time.deltaTime;
        }

        public void SetCollision(bool enabled)
        {
            gameObject.layer = enabled ? 8 : 0;
        }

        private void CheckHorizontalDirection()
        {
            if (transform.position.x <= _initialPosition.x + leftBound)
            {
                HorizontalDirection = HorizontalDirection.Right;
            }
            else if (transform.position.x >= _initialPosition.x + rightBound)
            {
                HorizontalDirection = HorizontalDirection.Left;
            }
        }

        private void CheckVerticalDirection()
        {
            if (transform.position.y <= _initialPosition.y + bottomBound)
            {
                _currentYDirection = VerticalDirection.Up;
            }
            else if (transform.position.y >= _initialPosition.y + topBound)
            {
                _currentYDirection = VerticalDirection.Down;
            }
        }

        private float HorizontalSpeed()
        {
            var directionMultiplier = HorizontalDirection == HorizontalDirection.Left ? -1 : 1;
            return xSpeed * directionMultiplier;
        }

        private float VerticalSpeed()
        {
            var directionMultiplier = _currentYDirection == VerticalDirection.Down ? -1 : 1;
            return ySpeed * directionMultiplier;
        }
    }
}