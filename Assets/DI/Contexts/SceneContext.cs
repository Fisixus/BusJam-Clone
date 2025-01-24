using UnityEngine;

namespace DI.Contexts
{
    public class SceneContext : MonoBehaviour
    {
        private Container _sceneContainer = new();

        [SerializeField] private Installer[] _installers;

        private void Awake()
        {
            foreach (var installer in _installers)
            {
                installer.Install(_sceneContainer);
            }
        }

        public T Resolve<T>() where T : class
        {
            return _sceneContainer.Resolve<T>();
        }
    }
}