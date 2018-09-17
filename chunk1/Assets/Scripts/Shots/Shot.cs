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
        public int OwnerId;

        public float Damage = 1;
        public float Velocity = 10;

        private float _time;

        public Shot(int ownerId, Vector3 position, Vector3 targetPosition, float time)
        {
            OwnerId = ownerId;
            _time = time;
            Position = position;
            StartPosition = Position;
            TargetPosition = targetPosition;
            Direction = (TargetPosition - Position).normalized;
        }

        public void Update(float dt)
        {
            _time += dt;
            OldPosition = Position;
            Position = GetPositionDelta(dt);
        }

        public Vector3 GetPosition(float time)
        {
            return GetPositionDelta(time - _time);
        }

        public Vector3 GetPositionDelta(float dt)
        {
            return Position + Direction * Velocity * dt;
        }
    }
}
