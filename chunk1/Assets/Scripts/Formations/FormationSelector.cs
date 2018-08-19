using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Commands;
using Assets.Scripts.Movement;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Formations
{
    public class FormationSelector
    {
        public static FormationType GetFormationType(
            IEnumerable<Unit> units, Vector3 target, CommandType commandType)
        {
            if (Neighbourhood.IsNeighbours(units))
            {
                if (units.Any(x => x.Navigation.IsUnitReachedTarget(target)))
                    return FormationType.Circle;
                else
                    return FormationType.Croud;
            }

            return FormationType.None;
        }
    }
}
