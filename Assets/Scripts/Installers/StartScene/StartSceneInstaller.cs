using DI;
using UnityEngine;

namespace Installers.StartScene
{
    public class StartSceneInstaller : Installer
    {
        [SerializeField] private MVPStartInstaller _mvpStartInstaller;

        protected override void InstallBindings()
        {
            _mvpStartInstaller.Install(Container);
        }
    }
}