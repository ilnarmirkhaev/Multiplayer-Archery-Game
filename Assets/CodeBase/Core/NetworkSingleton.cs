using Unity.Netcode;
using UnityEngine;

namespace CodeBase.Core
{
    public class NetworkSingleton<T> : NetworkBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance is null)
                {
                    T[] objects = FindObjectsOfType<T>();
                    
                    if (objects.Length > 0)
                        _instance = objects[0];
                    
                    if (objects.Length > 1)
                        Debug.LogError("There is more than one " + typeof(T).Name + " in the scene!");

                    if (_instance is null)
                    {
                        GameObject obj = new GameObject
                        {
                            name = $"_{typeof(T).Name}"
                        };
                        _instance = obj.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }
    }
}