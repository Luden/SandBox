using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Commands;
using Assets.Scripts.Core;

namespace Assets.Scripts.Formations
{
    public class FormationFactory : Pool<FormationType, Formation>
    {
        public FormationType GetFormationType(CommandType commandType)
        {
            return FormationType.Movement;
        }
    }
}
