using UnityEngine;

namespace Assets.Scripts
{
    public interface IUnitVelocity
    {
        float VelocityX { get; }
        float VelocityY { get; }

        Vector3 ExternalVelocity { get; }

        bool IsGrounded { get; }
    }
}
