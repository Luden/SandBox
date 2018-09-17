using Assets.Scripts.Core;
using Assets.Scripts.Movement;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class TargetFinder
    {
        private UnitManager _unitManager;
        private TimeManager _timeManager;

        private Targeting _targeting;
        private Aimer _aimer;
        private Navigation _navigation;
        private float _range;
        private int _ownerId;
        private RegularUpdate _regularUpdate;

        public TargetFinder(Targeting targeting, Aimer aimer, Navigation navigation, float range, int ownerId)
        {
            _targeting = targeting;
            _navigation = navigation;
            _range = range;
            _aimer = aimer;
            _ownerId = ownerId;
            _unitManager = ManagerProvider.Instance.UnitManager;
            _timeManager = ManagerProvider.Instance.TimeManager;

            _timeManager.StartUpdate(ref _regularUpdate, Update, 0.5f);
        }

        private void Update(float dt)
        {
            if (_targeting.IsUnitTarget())
                return;

            var target = FindTarget();
            if (target != null)
                _targeting.SetTarget(target);
        }

        Collider[] _colliders = new Collider[100];
        public Unit FindTarget()
        {
            double maxWeight = 0;
            Unit target = null;

            var count = Physics.OverlapSphereNonAlloc(_navigation.Position, _range, _colliders, LayerMasks.Units);
            for (int i = 0; i < count; i++)
            {
                var unitObj = _colliders[i].GetComponent<UnitObject>();
                if (unitObj.Owner.Id == _ownerId)
                    continue;

                if (unitObj.Owner.Hull.IsDead)
                    continue;

                var weight = CalculateWeight(unitObj.Owner);
                if (weight > maxWeight)
                {
                    maxWeight = weight;
                    target = unitObj.Owner;
                }
            }

            return target;
        }

        private int Group(float value, int parts)
        {
            return (int)(Mathf.Clamp01(value) * parts);
        }

        private double CalculateWeight(Unit candidate)
        {
            double result = 0;
            if (_targeting.CurrentTarget == candidate)
                return double.MaxValue;

            var targetPitch = _aimer.CalculatePitch(candidate.Navigation.Position);
            var remainingPitch = Mathf.Abs(Euler.Diff(_aimer.TotalPitch, targetPitch));
            var relativeRemainingPitch = remainingPitch / 180;
            result += Group(1 - relativeRemainingPitch, 4);
            result *= 10;

            var distance = Vector3.Distance(candidate.Navigation.Position, _navigation.Position);
            var relativeDistance = distance / _range;
            result += Group(1 - relativeDistance, 3);
            result *= 10;

            return result;
        }

        public void Deinit()
        {
            _timeManager.StopUpdate(ref _regularUpdate);
        }
    }
}
