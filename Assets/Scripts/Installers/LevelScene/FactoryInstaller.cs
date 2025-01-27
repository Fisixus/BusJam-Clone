using Core.Actors;
using Core.Factories;
using Core.Factories.Interface;
using DI;
using UnityEngine;
using Grid = Core.Actors.Grid;

namespace Installers.LevelScene
{
    public class FactoryInstaller : Installer
    {
        // Reference to the scene object
        [SerializeField] private DummyFactory _dummyFactory;
        [SerializeField] private GridFactory _gridFactory;
        [SerializeField] private BusFactory _busFactory;
        [SerializeField] private BusWaitingSpotFactory _waitingSpotFactory;

        protected override void InstallBindings()
        {
            Container.BindAsSingle<IDummyFactory>(() => _dummyFactory);
            Container.BindAsSingle<IGridFactory>(() => _gridFactory);
            Container.BindAsSingle<IBusFactory>(() => _busFactory);
            Container.BindAsSingle<IBusWaitingSpotFactory>(() => _waitingSpotFactory);
        }
    }
}