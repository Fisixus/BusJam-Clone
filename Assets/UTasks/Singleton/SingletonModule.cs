using UnityEngine;

namespace UTasks.Singleton
{
    public class SingletonModule<T> : MonoBehaviour where T : SingletonModule<T>
    {
        private static bool isGeneratedBefore;

        private static T instance;

        public static T Instance
        {
            get
            {
                if (isGeneratedBefore) return instance;
                if (instance == null)
                {
                    var instances = FindObjectsOfType<T>();
                    if (instances.Length == 0)
                        CreateSingletonModule();
                    else
                        instance = instances[0];
                }

                isGeneratedBefore = true;
                return instance;
            }
            set => instance = value;
        }


        private static void CreateSingletonModule()
        {
            var newModule = new GameObject();
            instance = newModule.AddComponent<T>();
            newModule.name = instance.name + "_Module";
            if (Application.isPlaying)
                DontDestroyOnLoad(newModule);
        }

        public bool IsInstanceSetupBefore(bool liveForever = true)
        {
            if (instance != null && instance != this)
            {
                Destroy(instance.gameObject);
                return true;
            }

            instance = (T)this;
            if (liveForever)
                DontDestroyOnLoad(instance.gameObject);

            return false;
        }
    }
}