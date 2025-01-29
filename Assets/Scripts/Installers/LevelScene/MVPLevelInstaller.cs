using DI;
using MVP.Models;
using MVP.Models.Interface;
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
            Container.BindAsSingle<IBusModel>(() => Container.Construct<BusModel>());
            Container.BindAsSingle<IStationModel>(() => Container.Construct<StationModel>());
            Container.BindAsSingle<ILevelUIView>(() => _levelUIView);
            Container.BindAsSingleNonLazy(() => Container.Construct<StationPresenter>());
            Container.BindAsSingleNonLazy(() => Container.Construct<LevelPresenter>());
        }
    }
}