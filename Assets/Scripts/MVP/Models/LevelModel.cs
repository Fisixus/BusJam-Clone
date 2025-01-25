using Core.LevelSerialization;
using MVP.Models.Interface;
using UnityEngine;

namespace MVP.Models
{
    public class LevelModel : ILevelModel
    {
        public int MaxLevel => _levelFiles?.Length ?? 0; // Return 1 if jsonFiles is null

        public int LevelIndex
        {
            get
            {
                var levelIndex = Mathf.Max(1, PlayerPrefs.GetInt(LevelIndexStr));
                return levelIndex;
            }
            set
            {
                PlayerPrefs.SetInt(LevelIndexStr, value);
                PlayerPrefs.Save();
            }
        }

        private const string LevelIndexStr = "LevelIndex";
        private TextAsset[] _levelFiles;

        public LevelModel()
        {
            _levelFiles = Resources.LoadAll<TextAsset>("Levels");
        }

        ~LevelModel()
        {
        }

        public LevelInfo LoadLevel()
        {
            var currentLevelInfo = LevelSerializer.SerializeToLevelInfo(LevelIndex);
            if (currentLevelInfo is null)
            {
                ResetLevelIndex();
                currentLevelInfo = LevelSerializer.SerializeToLevelInfo(LevelIndex);
            }

            return currentLevelInfo;
        }

        public LevelInfo LoadLevel(int levelIndex)
        {
            var currentLevelInfo = LevelSerializer.SerializeToLevelInfo(levelIndex);
            if (currentLevelInfo is null)
            {
                Debug.Log("Invalid level Index!");
                return null;
            }

            return currentLevelInfo;
        }

        private void ResetLevelIndex()
        {
            LevelIndex = 1;
        }
    }
}