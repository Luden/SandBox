using System;
using UnityEngine;

namespace Assets.Scripts
{
    class Selectable : MonoBehaviour, ISelectable
    {
        public Action<bool> OnSelectionChange { get; set; }
        public Action<bool> OnPreselectionChange { get; set; }

        private Transform _transform;

        public float Radius { get { return 0.5f; } }

        bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    OnSelectionChange(_selected);
                }
            }
        }

        bool _preselected;
        public bool Preselected
        {
            get { return _preselected; }
            set
            {
                if (_preselected != value)
                {
                    _preselected = value;
                    OnPreselectionChange(_preselected);
                }
            }
        }

        void Start()
        {
            _transform = GetComponent<Transform>();
        }

        public Vector3 Position { get { return _transform.position; } }
    }
}
