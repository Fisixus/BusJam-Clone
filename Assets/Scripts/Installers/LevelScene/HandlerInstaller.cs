using DI;
using MVP.Presenters.Handlers;

namespace Installers.LevelScene
{
    public class HandlerInstaller : Installer
    {
        protected override void InstallBindings()
        {
            Container.BindAsSingle(() => Container.Construct<LevelSetupHandler>());
            Container.BindAsSingle(() => Container.Construct<GridEscapeHandler>());
            Container.BindAsSingle(() => Container.Construct<BusSystemHandler>());
        }
    }
}