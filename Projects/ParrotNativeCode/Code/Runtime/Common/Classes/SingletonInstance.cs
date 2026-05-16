using UnityEngine;

namespace ParrotCode.Native.Common
{
    [DisallowMultipleComponent]
    public class SingletonInstance<T>: BaseMonoBehavior where T : MonoBehaviour
    {
        [SerializeField, Space(5)]
        protected bool doNotDestroyOnLoad = true;

        private static T instance;
        public static T Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                        instance = new GameObject($"{nameof(T)} [Singleton Instance]").AddComponent<T>();
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (!doNotDestroyOnLoad)
                return;

            if(instance !=null && instance != this)
                Destroy(instance);

            DontDestroyOnLoad(this);
        }
    }
}
