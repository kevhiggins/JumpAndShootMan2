using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Abilities
{
    [RequireComponent(typeof(IUnitVelocity))]
    public class HorizontalMoveAbility : MonoBehaviour
    {
        public float runSpeed = 8f;
        public float groundDamping = 20f; // how fast do we change direction? higher means faster
        public float inAirDamping = 5f;

        public UnityEvent OnGroundedMove = new UnityEvent();
        public UnityEvent OnGroundedIdle = new UnityEvent();

        private float _normalizedSpeed = 0;

        public float Speed
        {
            get
            {
                var smoothedMovementFactor = _unitVelocity.IsGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
                return Mathf.Lerp(_unitVelocity.VelocityX, _normalizedSpeed * runSpeed + _unitVelocity.ExternalVelocity.x, Time.deltaTime * smoothedMovementFactor);
            }
        }

        private IUnitVelocity _unitVelocity;

        void Awake()
        {
            _unitVelocity = GetComponent<IUnitVelocity>();
        }

        public void TryLeft()
        {
            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            _normalizedSpeed = -1;
            CheckOnGroundedMove();
        }

        public void TryRight()
        {
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            _normalizedSpeed = 1;
            CheckOnGroundedMove();
        }

        public void TryIdle()
        {
            _normalizedSpeed = 0;
            if (_unitVelocity.IsGrounded)
            {
                OnGroundedIdle.Invoke();
            }
        }

        private void CheckOnGroundedMove()
        {
            if (_unitVelocity.IsGrounded)
            {
                OnGroundedMove.Invoke();
            }
        }
    }
}
