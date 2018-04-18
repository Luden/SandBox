using System;
using UnityEngine;

namespace Assets.Scripts
{
    public interface ISelectable
    {
        bool Selected { get; set; }
        Action<bool> OnSelectionChange { get; set; }
        Vector3 Position { get; }
    }
}
