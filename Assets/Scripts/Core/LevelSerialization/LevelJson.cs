using System;

namespace Core.LevelSerialization
{
    /// <summary>
    /// 
    /// LevelJson is a class that is used to store the information of a level.
    /// It is serializable and compatible with JSON format to be used in the game level system.
    /// 
    /// </summary>
    [Serializable]
    public class LevelJson
    {
        public int level_number;
        public int grid_width;
        public int grid_height;
        public int timer;
        public string[] grid;
        public string[] bus_order;
    }
}
