using Core.LevelSerialization;
using JetBrains.Annotations;

namespace MVP.Models.Interface
{
    public interface ILevelModel
    {
        public int LevelIndex { get; set; }
        public int MaxLevel { get; }

        public LevelInfo LoadLevel();

        [CanBeNull]
        public LevelInfo LoadLevel(int index);
    }
}