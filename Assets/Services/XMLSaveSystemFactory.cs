

namespace Assets.Services
{
    class XMLSaveSystemFactory : SaveSystemFactory
    {
        public override ISaveSystem GetSaveSystem()
        {
            return new XMLSaveSystem();
        }
    }
}
