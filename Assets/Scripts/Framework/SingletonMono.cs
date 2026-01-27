using System;
using System.Collections;
using UnityEngine;

namespace Framework
{
    [DefaultExecutionOrder(90)]
    public class SingletonMono<T> : MonoBehaviour where T : class
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (!Application.isPlaying)
                {
                    return null;
                }

                if (_instance == null)
                {
                    // 先查找是否已存在
                    _instance = FindObjectOfType(typeof(T)) as T;

                    if (_instance == null)
                    {
                        // GameObject root = GameObject.Find("SingletonMono");
                        // if (root == null)
                        //     return null;
                        
                        GameObject go = new GameObject(typeof(T).ToString());
                        _instance = go.AddComponent(typeof(T)) as T;
                        DontDestroyOnLoad(go);
                        
                        // go.transform.parent = root.transform;
                    }
                }

                return _instance;
            }
        }

        public virtual void OnDestroy()
        {
            if (_instance != null && _instance.Equals(this))
            {
                _instance = null;
            }
        }

        public static T GetInstance()
        {
            return Instance;
        }

        public static bool haveInstance
        {
            get
            {
                GameObject root = GameObject.Find("SingletonMono");
                if (root == null)
                {
                    return false;
                }

                Transform trans = root.transform.Find(typeof(T).ToString());
                return trans != null;
            }
        }

        public static bool IsInstanceValid()
        {
            return _instance != null;
        }

        protected virtual void OnApplicationQuit()
        {
            // _instance = null;
        }
#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInitialize()
        {
            _instance = null;
        }
#endif
    }
}