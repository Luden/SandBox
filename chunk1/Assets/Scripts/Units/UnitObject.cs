using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Units
{
    // interface only to remove a lot of MonoBehaviour's stuff from intellisence. I don't need such complexity, I don't care!
    public interface IUnitObject 
    {
        NavMeshAgent NavMeshAgent { get; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        Action<IUnitObject> OnColliderEnter { get; set; }
        Action<IUnitObject> OnColliderExit { get; set; }
        Unit Owner { get; } // cyclic refs for the win! because fuck you thats why
    }

    public class UnitObject : MonoBehaviour, IUnitObject
    {
        public NavMeshAgent NavMeshAgent { get; private set; }
        public Action<IUnitObject> OnColliderEnter { get; set; }
        public Action<IUnitObject> OnColliderExit { get; set; }
        public Vector3 Position { get { return transform.position; } }
        public Quaternion Rotation { get { return transform.rotation; } }
        public Unit Owner { get; private set; }

        private void Awake()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void Init(Unit unit)
        {
            Owner = unit;
        }

        void OnTriggerEnter(Collider other)
        {
            var unit = other.GetComponent<UnitObject>();
            if (unit != null && OnColliderEnter != null)
                OnColliderEnter(unit);
        }

        void OnTriggerExit(Collider other)
        {
            var unit = other.GetComponent<UnitObject>();
            if (unit != null && OnColliderExit != null)
                OnColliderExit(unit);
        }
    }
}
