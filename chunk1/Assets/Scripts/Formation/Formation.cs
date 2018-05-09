using System.Collections.Generic;
using Assets.Scripts.Core;

namespace Assets.Scripts.Formations
{
    public class Formation : IKeyProvider<FormationType>
    {
        public List<Unit> _units = new List<Unit>();

        public FormationType GetKey()
        {
            return FormationType.Movement;
        }

        public void Init(IEnumerable<Unit> units)
        {
            _units.Clear();
            _units.AddRange(units);
        }
    }
}
