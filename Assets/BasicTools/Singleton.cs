using UnityEngine;


namespace BasicTools
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public virtual void Awake()
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
                        Debug.Log("[Singleton]: object of '" +typeof(T)+ "' doesn't exist in scene");
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


