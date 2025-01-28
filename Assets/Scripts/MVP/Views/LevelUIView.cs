using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DI.Contexts;
using MVP.Presenters;
using MVP.Presenters.Handlers;
using MVP.Views.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVP.Views
{
    public class LevelUIView : MonoBehaviour, ILevelUIView
    {
        [field: SerializeField] public TextMeshProUGUI LevelText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TimerText { get; private set; }
        [field: SerializeField] public Button RetryLevelButton { get; private set; }
        [field: SerializeField] public Button NextLevelButton { get; private set; }
        [field: SerializeField] public Transform SuccessPanelTr { get; private set; }
        [field: SerializeField] public Transform FailPanelTr { get; private set; }
        
        private void Awake()
        {
            RetryLevelButton.onClick.AddListener(() => { RetryLevel().Forget(); });
            NextLevelButton.onClick.AddListener(() => { NextLevel().Forget(); });
        }

        private void OnDisable()
        {
            RetryLevelButton.onClick.RemoveAllListeners();
            NextLevelButton.onClick.RemoveAllListeners();
        }
        private async UniTask RetryLevel()
        {
            try
            {
                // Disable the button to prevent multiple clicks
                RetryLevelButton.interactable = false;

                // Resolve dependencies
                var scenePresenter = ProjectContext.Container.Resolve<ScenePresenter>();
                var levelTransitionHandler = ProjectContext.Container.Resolve<SceneTransitionHandler>();

                // Perform scene transition
                Debug.Log("Starting transition to LevelScene...");
                await scenePresenter.TransitionToNextScene("LevelScene",
                    async (container) =>
                    {
                        // Specific setup logic for this scene
                        await levelTransitionHandler.SetupLevelSceneRequirements(container);
                    });
                Debug.Log("Transition to LevelScene complete!");
            }
            catch (Exception e)
            {
                throw new Exception("Error in RetryLevel transition", e);
            }
        }
        
        public async UniTask NextLevel()
        {
            try
            {
                // Disable the button to prevent multiple clicks
                NextLevelButton.interactable = false;

                // Resolve dependencies
                var scenePresenter = ProjectContext.Container.Resolve<ScenePresenter>();
                var levelTransitionHandler = ProjectContext.Container.Resolve<SceneTransitionHandler>();

                // Perform scene transition
                Debug.Log("Starting transition to StartScene...");
                await scenePresenter.TransitionToNextScene("StartScene",
                    async (container) =>
                    {
                        // Specific setup logic for this scene
                        await levelTransitionHandler.SetupStartSceneRequirements(container);
                    });
                Debug.Log("Transition to StartScene complete!");
            }
            catch (Exception e)
            {
                throw new Exception("Error in NextLevel transition", e);
            }
        }

        public void OpenSuccessPanel(float duration)
        {
            TogglePanel(SuccessPanelTr, true, duration).Forget();
        }
        public void CloseSuccessPanel(float duration)
        {
            TogglePanel(SuccessPanelTr, false, duration).Forget();
        }

        public void OpenFailPanel(float duration)
        {
            TogglePanel(FailPanelTr, true, duration).Forget();
        }

        public void CloseFailPanel(float duration)
        {
            TogglePanel(FailPanelTr, false, duration).Forget();
        }
        
        private async UniTask TogglePanel(Transform panelTransform, bool isOpen, float duration)
        {
            var cg = panelTransform.GetComponent<CanvasGroup>();
            if (isOpen)
            {
                cg.interactable = true;
                cg.blocksRaycasts = true;
                DOTween.To(() => cg.alpha, x => cg.alpha = x, 1, duration);
            }
            else
            {
                cg.alpha = 0;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(duration), DelayType.DeltaTime);
        }
    }
}
