using System.Collections.Generic;
using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class UnitObjectManager : MonoBehaviour, IManager
    {
        [SerializeField] List<UnitObject> _prefabs = new List<UnitObject>();

        public ManagerType ManagerType { get { return ManagerType.UnitObject; } }

        public Dictionary<int, IUnitObject> Units = new Dictionary<int, IUnitObject>();

        public void Init()
        {
        }

        public IUnitObject CreateUnitObject()
        {
            var prefab = _prefabs[0];
            var unitObject = GameObject.Instantiate<UnitObject>(prefab, gameObject.transform);
            return unitObject;
        }
    }
}
