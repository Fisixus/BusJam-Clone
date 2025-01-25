using DI;
using MVP.Presenters;
using MVP.Views;
using MVP.Views.Interface;
using UnityEngine;

namespace Installers.LevelScene
{
    public class MVPLevelInstaller : Installer
    {
        [SerializeField] private LevelUIView _levelUIView;

        protected override void InstallBindings()
        {
            Container.BindAsSingle<ILevelUIView>(() => _levelUIView);
            //Container.BindAsSingleNonLazy(() => Container.Construct<LevelPresenter>());
        }
    }
}
