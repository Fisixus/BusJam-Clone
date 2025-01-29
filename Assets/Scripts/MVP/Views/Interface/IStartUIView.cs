using TMPro;
using UnityEngine.UI;

namespace MVP.Views.Interface
{
    public interface IStartUIView
    {
        public Button NewLevelButton { get; }
        public TextMeshProUGUI LevelButtonText { get; }

        public void SetLevelButtonText(int level, int maxLevel);
    }
}