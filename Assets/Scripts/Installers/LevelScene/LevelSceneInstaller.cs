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
            Container.BindAsSingle(() => Container.Construct<ScenePresenter>());
            //Container.BindAsSingle(() => Container.Construct<SceneTransitionHandler>());
            Container.BindAsSingle<ILevelModel>(() => Container.Construct<LevelModel>());
            _handlerInstaller.Install(Container);
            _factoryInstaller.Install(Container);
            _mvpLevelInstaller.Install(Container);
            
            Container.BindAsSingleNonLazy(() => Container.Construct<BusPresenter>());
            Container.BindAsSingleNonLazy(() => Container.Construct<GridPresenter>());
            Container.BindAsSingleNonLazy(() => Container.Construct<LevelPresenter>());
            Container.BindAsSingleNonLazy(() => Container.Construct<GamePresenter>());
            

            
        }
    }
}