using UnityEngine;

namespace Assets.Services
{
    public abstract class SaveSystemFactory:ScriptableObject
    {
        public abstract ISaveSystem GetSaveSystem();

        public abstract ISaveSystem GetChachedSaveSystem();
    }
}
