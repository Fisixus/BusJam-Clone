using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVP.Views.Interface
{
    public interface ILevelUIView
    {
        public Button NextLevelButton { get; }
        public Button RetryLevelButton { get; }
        public TextMeshProUGUI LevelText { get; }
        public TextMeshProUGUI TimerText { get;  }
        public Transform SuccessPanelTr { get; }
        public Transform FailPanelTr { get; }

        public void OpenSuccessPanel(float duration = 1f);
        public void CloseSuccessPanel(float duration = 0f);
        public void OpenFailPanel(float duration = 1f);
        public void CloseFailPanel(float duration = 0f);
    }
}
