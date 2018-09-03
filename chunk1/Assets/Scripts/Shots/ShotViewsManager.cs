using System.Collections.Generic;
using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Shots
{
    public class ShotViewsManager : MonoBehaviour, IManager
    {
        [SerializeField] private List<ShotView> _shotViewPrefabs = new List<ShotView>();

        public ManagerType ManagerType { get { return ManagerType.ShotViews; } }

        private ShotsManager _shotsManager;
        private Dictionary<Shot, ShotView> _shotViews = new Dictionary<Shot, ShotView>();

        public void Init()
        {
            _shotsManager = ManagerProvider.Instance.ShotsManager;
            _shotsManager.OnShotCreated += OnShotCreated;
            _shotsManager.OnShotRemoved += OnShotRemoved;
        }

        private ShotView CreateShotView(Shot shot)
        {
            var prefab = _shotViewPrefabs[0];
            return Instantiate<ShotView>(prefab, shot.Position, Quaternion.LookRotation(shot.Direction), transform); 
        }

        private void OnShotCreated(Shot shot)
        {
            var shotView = CreateShotView(shot);
            shotView.Init(shot);
            _shotViews[shot] = shotView;
        }

        private void OnShotRemoved(Shot shot)
        {
            ShotView shotView;
            _shotViews.TryGetValue(shot, out shotView);
            if (shotView == null)
                return;

            _shotViews.Remove(shot);
            Destroy(shotView.gameObject);
        }
    }
}
