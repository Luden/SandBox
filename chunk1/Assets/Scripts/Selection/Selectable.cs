using System;
using Assets.Scripts.Core;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts
{
    public class Selectable
    {
        public Action<bool> OnSelectionChange { get; set; }
        public Action<bool> OnPreselectionChange { get; set; }

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
                    if (OnSelectionChange != null)
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
                    if (OnPreselectionChange != null)
                        OnPreselectionChange(_preselected);
                }
            }
        }
    }
}
