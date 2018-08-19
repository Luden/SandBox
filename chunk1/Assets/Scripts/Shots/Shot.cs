using UnityEngine;

namespace Assets.Scripts.Shots
{
    public class Shot
    {
        public Vector3 OldPosition;
        public Vector3 Position;
        public Vector3 StartPosition;
        public Vector3 TargetPosition;
        public Vector3 Direction;

        public float Damage;
        public float Velocity;

        public Shot(Vector3 position, Vector3 targetPosition)
        {
            Position = position;
            StartPosition = Position;
            TargetPosition = targetPosition;
            Direction = (TargetPosition - Position).normalized;
        }

        public void Update(float dt)
        {
            OldPosition = Position;
            Position = Position + Direction * Velocity * dt;
        }
    }
}
