using System;
using System.Text;
using Cysharp.Threading.Tasks;
using DI.Contexts;
using MVP.Presenters;
using MVP.Presenters.Handlers;
using MVP.Views.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVP.Views
{
    public class StartUIView : MonoBehaviour, IStartUIView
    {
        [field: SerializeField] public Button NewLevelButton { get; private set; }
        [field: SerializeField] public TextMeshProUGUI LevelButtonText { get; private set; }

        private void Awake()
        {
            NewLevelButton.onClick.AddListener(() => { RequestLevel().Forget(); });
        }

        private void OnDisable()
        {
            NewLevelButton.onClick.RemoveAllListeners();
        }

        private async UniTask RequestLevel()
        {
            try
            {
                // Disable the button to prevent multiple clicks
                NewLevelButton.interactable = false;

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
            catch (Exception ex)
            {
                Debug.LogError($"Error during scene transition: {ex.Message}");
            }
        }


        public void SetLevelButtonText(int level, int maxLevel)
        {
            StringBuilder stringBuilder;
            if (level > maxLevel)
            {
                NewLevelButton.interactable = false;
                stringBuilder = new StringBuilder("Finished");
                NewLevelButton.onClick.RemoveAllListeners();
            }
            else
            {
                NewLevelButton.interactable = true;
                stringBuilder = new StringBuilder("Level ");
                stringBuilder.Append(level);
            }

            LevelButtonText.text = stringBuilder.ToString();
        }
    }
}