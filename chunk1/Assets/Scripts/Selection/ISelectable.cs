using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
