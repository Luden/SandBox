using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Core;

namespace Assets.Scripts.Perception
{
    public class VisibilityManager : IManager
    {
        public ManagerType ManagerType { get { return ManagerType.Visibility; } }

        public void Init()
        {
        }
    }
}
