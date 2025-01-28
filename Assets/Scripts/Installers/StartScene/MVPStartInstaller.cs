using DI;
using MVP.Views;
using MVP.Views.Interface;
using UnityEngine;

namespace Installers.StartScene
{
    public class MVPStartInstaller : Installer
    {
        [SerializeField] private StartUIView _startUIView;

        protected override void InstallBindings()
        {
            Container.BindAsSingle<IStartUIView>(() => _startUIView);
        }
    }
}