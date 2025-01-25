using DI;
using MVP.Models;
using MVP.Models.Interface;
using MVP.Presenters;

namespace Installers
{
    public class MVPProjectInstaller : Installer
    {
        protected override void InstallBindings()
        {
            Container.BindAsSingle(() => Container.Construct<ScenePresenter>());
            //Container.BindAsSingle(() => Container.Construct<SceneTransitionHandler>());
            Container.BindAsSingle<ILevelModel>(() => Container.Construct<LevelModel>());
            Container.BindAsSingleNonLazy(() => Container.Construct<LevelPresenter>());
            Container.BindAsSingleNonLazy(() => Container.Construct<GamePresenter>());
        }
    }
}