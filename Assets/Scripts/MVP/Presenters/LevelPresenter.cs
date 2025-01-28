using System;
using Core.LevelSerialization;
using Cysharp.Threading.Tasks;
using DI.Contexts;
using MVP.Models.Interface;
using MVP.Presenters.Handlers;
using MVP.Views.Interface;
using UnityEngine.SceneManagement;
using UTasks;

namespace MVP.Presenters
{
    public class LevelPresenter
    {
        private readonly LevelSetupHandler _levelSetupHandler;
        private readonly GoalHandler _goalHandler;
        private readonly ILevelModel _levelModel;
        private readonly ILevelUIView _levelUIView;

        public LevelPresenter(LevelSetupHandler levelSetupHandler, GoalHandler goalHandler, ILevelUIView levelUIView)
        {
            _levelSetupHandler = levelSetupHandler;
            _goalHandler = goalHandler;
            _levelUIView = levelUIView;
            
            _levelModel = ProjectContext.Container.Resolve<ILevelModel>();
            
            _goalHandler.OnLevelCompleted += HandleLevelCompleted;
            _goalHandler.OnLevelFailed += HandleLevelFailed;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            Dispose();
        }

        private void Dispose()
        {
            _goalHandler.OnLevelCompleted -= HandleLevelCompleted;
            _goalHandler.OnLevelFailed -= HandleLevelFailed;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void HandleLevelCompleted()
        {
            _levelModel.LevelIndex++;
            UTask.Wait(0.5f).Do(() => { _levelUIView.OpenSuccessPanel();});
        }

        private void HandleLevelFailed()
        {
            UTask.Wait(0.5f).Do(() => { _levelUIView.OpenFailPanel();});
        }

        public async UniTask LoadLevel()
        {
            _levelUIView.LevelText.text = $"LEVEL {_levelModel.LevelIndex}";
            var levelInfo = _levelModel.LoadLevel();
            _levelSetupHandler.Initialize(levelInfo);
            _goalHandler.Initialize(levelInfo.BusOrder.Length, levelInfo.Timer);
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f), DelayType.DeltaTime);
        }

        public void LoadFromLevelEditor(LevelInfo levelInfo)
        {
            //_gridView.CalculateGridSize(levelInfo.GridSize);
            _levelSetupHandler.Initialize(levelInfo);
            //_goalHandler.Initialize(levelInfo.Goals, levelInfo.NumberOfMoves);
        }
    }
}