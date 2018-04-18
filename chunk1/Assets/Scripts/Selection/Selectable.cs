using System;
using UnityEngine;

namespace Assets.Scripts
{
    class Selectable : MonoBehaviour, ISelectable
    {
        public Action<bool> OnSelectionChange { get; set; }

        private Transform _transform;

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

        void Start()
        {
            _transform = GetComponent<Transform>();
        }

        public Vector3 Position { get { return _transform.position; } }
    }
}
