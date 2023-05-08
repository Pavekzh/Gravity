using UnityEngine;

namespace Assets.Services
{
    [CreateAssetMenu(fileName = "XMLSaveSystemFactory", menuName = "ScriptableObjects/XMLSaveSystemFactory")]
    class XMLSaveSystemFactory : SaveSystemFactory
    {
        private XMLSaveSystem cachedSaveSystem;

        public override ISaveSystem GetChachedSaveSystem()
        {
            if (cachedSaveSystem == null)
                cachedSaveSystem = new XMLSaveSystem();
            return cachedSaveSystem;
        }

        public override ISaveSystem GetSaveSystem()
        {
            return new XMLSaveSystem();
        }
    }
}
