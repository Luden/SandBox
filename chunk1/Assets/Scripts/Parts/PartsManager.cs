using Assets.Scripts.Core;

namespace Assets.Scripts.Parts
{
    public class PartsManager : IManager
    {
        public ManagerType ManagerType { get { return ManagerType.Parts; } }

        private PartsFactory _partsFactory;

        public void Init()
        {
            _partsFactory = new PartsFactory();
        }

        public Part CreatePart(PartType partType)
        {
            return _partsFactory.Create(partType);
        }
    }
}
