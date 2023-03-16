using UnityEngine;

namespace Assets.Services
{
    public abstract class SaveSystemFactory:MonoBehaviour
    {
        public abstract ISaveSystem GetSaveSystem();

        public abstract ISaveSystem GetChachedSaveSystem();
    }
}
