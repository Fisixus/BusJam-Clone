using System;
using Core.LevelSerialization;
using Cysharp.Threading.Tasks;
using DI.Contexts;
using MVP.Models.Interface;
using MVP.Presenters.Handlers;
using MVP.Views.Interface;
using UnityEngine.SceneManagement;

namespace MVP.Presenters
{
    public class LevelPresenter
    {
        private readonly LevelSetupHandler _levelSetupHandler;
        //private readonly GoalHandler _goalHandler;
        //private readonly IGridView _gridView;
        private readonly ILevelModel _levelModel;
        private readonly ILevelUIView _levelUIView;

        public LevelPresenter(LevelSetupHandler levelSetupHandler, ILevelUIView levelUIView, ILevelModel levelModel)
        {
            _levelSetupHandler = levelSetupHandler;
            //_goalHandler = goalHandler;
            _levelUIView = levelUIView;
            _levelModel = levelModel;
            //_levelModel = ProjectContext.Container.Resolve<ILevelModel>();
            LoadLevel();//TODO:
            //_goalHandler.OnLevelCompleted += HandleLevelCompleted;
            //_goalHandler.OnLevelFailed += HandleLevelFailed;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            Dispose();
        }

        private void Dispose()
        {
            //_goalHandler.OnLevelCompleted -= HandleLevelCompleted;
            //_goalHandler.OnLevelFailed -= HandleLevelFailed;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void HandleLevelCompleted()
        {
            _levelModel.LevelIndex++;
            //UTask.Wait(0.25f).Do(() => { _levelUIView.OpenSuccessPanel(); });
        }

        private void HandleLevelFailed()
        {
            //UTask.Wait(0.25f).Do(() => { _levelUIView.OpenFailPanel(); });
        }

        public async UniTask LoadLevel()
        {
            //var levelModel = ProjectContext.Container.Resolve<ILevelModel>();
            var levelInfo = _levelModel.LoadLevel();
            //_gridView.CalculateGridSize(levelInfo.GridSize);
            _levelSetupHandler.Initialize(levelInfo);
            //_goalHandler.Initialize(levelInfo.Goals, levelInfo.NumberOfMoves);
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