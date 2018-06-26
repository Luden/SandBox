using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Commands;
using Assets.Scripts.Core;

namespace Assets.Scripts.Formations
{
    public class FormationFactory : Pool<FormationType, FormationBase>
    {
        protected override FormationBase Create(FormationType key)
        {
            switch (key)
            {
                case FormationType.Circle:
                    return new CircleFormation();
                case FormationType.Croud:
                    return new CroudFormation();
                case FormationType.None:
                    return new NoneFormation();
                default:
                    return null;
            }
        }
    }
}
