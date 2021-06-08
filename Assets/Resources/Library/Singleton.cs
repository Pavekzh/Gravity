using UnityEngine;


namespace Assets.Resources.Library
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public void Awake()
        {
            if (!instance)
            {
                instance = gameObject.GetComponent<T>();
            }
            else
            {
                Debug.LogError("[Singleton]: Second instance of '" + typeof(T) + "' created!");
            }
            InstancesCount++;
        }
        private void OnDestroy()
        {
            InstancesCount--;
        }

        public static int InstancesCount;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));
                    InstancesCount = FindObjectsOfType(typeof(T)).Length;
                    if (InstancesCount > 1)
                    {
                        Debug.LogError("[Singleton]: multiple instances of '" + typeof(T) + "' found!");

                    }

                    if (instance == null)
                    {
                        // If there are no objects with this class in the scene, create
                        // a new GameObject and sculpt our component to it
                        GameObject singleton = new GameObject();
                        instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();
                        DontDestroyOnLoad(singleton);
                        Debug.Log("[Singleton]: An instance of '" + typeof(T) + "' was created: " + singleton);
                        InstancesCount++;
                    }
                    else
                    {
                        Debug.Log("[Singleton]: Using instance of '" + typeof(T) + "': " + instance.gameObject.name);
                    }
                }
                return instance;
            }
        }
    }
}


