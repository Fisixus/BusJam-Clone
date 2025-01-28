using DI;
using MVP.Models;
using MVP.Models.Interface;
using MVP.Presenters;
using UnityEngine;

namespace Installers.LevelScene
{
    public class LevelSceneInstaller : Installer
    {
        [SerializeField] private HandlerInstaller _handlerInstaller;
        [SerializeField] private MVPLevelInstaller _mvpLevelInstaller;
        [SerializeField] private FactoryInstaller _factoryInstaller;

        protected override void InstallBindings()
        {
            _handlerInstaller.Install(Container);
            _factoryInstaller.Install(Container);
            _mvpLevelInstaller.Install(Container);
        }
    }
}