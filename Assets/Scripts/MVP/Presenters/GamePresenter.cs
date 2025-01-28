using Cysharp.Threading.Tasks;
using MVP.Presenters.Handlers;
using UnityEngine.Device;

namespace MVP.Presenters
{
    public class GamePresenter
    {
        private readonly ScenePresenter _scenePresenter;
        private readonly SceneTransitionHandler _sceneTransitionHandler;

        public GamePresenter(ScenePresenter scenePresenter, SceneTransitionHandler sceneTransitionHandler)
        {
            Application.targetFrameRate = 60;
            _scenePresenter = scenePresenter;
            _sceneTransitionHandler = sceneTransitionHandler;
            InitializeGame().Forget();
        }

        private async UniTask InitializeGame()
        {
            await _scenePresenter.TransitionToNextScene("StartScene",
                async (container) => { await _sceneTransitionHandler.SetupStartSceneRequirements(container); });
        }
    }
}