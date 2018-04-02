using System;
using UnityEngine;

namespace Assets.Scripts
{
    public enum HitDirection { None, Top, Bottom, Forward, Back, Left, Right }
    public class HitDirectionResolver
    {
        private Vector3 _lastPosition;

        public void SavePosition(Vector3 lastPosition)
        {
            _lastPosition = lastPosition;
        }

        public HitDirection FindDirection(GameObject Object, Collider2D ObjectHit, LayerMask layerMask)
        {
            HitDirection hitDirection;
            Vector3 direction = (Object.transform.position - ObjectHit.transform.position).normalized;

            if (direction.x < 0 && _lastPosition.x < ObjectHit.bounds.min.x)
            {
                hitDirection = HitDirection.Left;
            }
            else if (direction.x > 0 && _lastPosition.x > ObjectHit.bounds.max.x)
            {
                hitDirection = HitDirection.Right;
            }
            else if (direction.y > 0 && _lastPosition.y > ObjectHit.bounds.max.y)
            {
                hitDirection = HitDirection.Top;
            }
            else if (direction.y < 0 && _lastPosition.y < ObjectHit.bounds.min.y)
            {
                hitDirection = HitDirection.Bottom;
            }
            else
            {
                throw new Exception("Collision found, but side could not be determined.");
            }

            return hitDirection;
        }
    }
}
