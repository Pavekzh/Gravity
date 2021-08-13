using System.Collections.Generic;
using UnityEngine;

namespace BasicTools
{
    public class DataStorage : Singleton<DataStorage>
    {
        // References to all stored data
        private Dictionary<string, object> storage = new Dictionary<string, object>();

        public void SaveData(string key, object data)
        {
            if (storage.ContainsKey(key)) // If something under key exist already we are printing warning.
            {
                Debug.LogWarningFormat("[{0}] Overriding value in: {1}.", typeof(DataStorage), key);
            }
            storage[key] = data;
        }

        public bool HasData<T>(string key)
        {
            if (!storage.ContainsKey(key)) // If storage doesn't has key then return false.
            {
                return false;
            }
            return ((T)storage[key]) != null; // If storage has data but we need to verify type.
        }

        public T GetData<T>(string key)
        {
            if (!storage.ContainsKey(key)) // Check is storage has data under provided key.
            {
                Debug.LogWarningFormat("[{0}] No value under key: {1}. Returning default", typeof(DataStorage), key);
                return default(T); // Return default value for type.
            }
            return (T)storage[key];
        }

        public void RemoveData(string key)
        {
            if (storage.ContainsKey(key)) // If data under provided key exist, we are removing it.
            {
                storage.Remove(key);
            }
        }
    }
}
