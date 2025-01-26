using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.LevelSerialization
{
    public static class LevelSerializer
    {
        public static LevelInfo SerializeToLevelInfo(int level)
        {
            try
            {
                TextAsset jsonFile = Resources.Load<TextAsset>("Levels/level_" + level.ToString("00"));
                string jsonString = jsonFile.text;
                var levelJson = JsonUtility.FromJson<LevelJson>(jsonString);
                var (dummies, busOrder) = ProcessLevelJson(levelJson);
                return new LevelInfo(levelJson.level_number, dummies, busOrder, levelJson.timer);
            }
            catch (Exception e)
            {
                Debug.Log("JSON error:" + e);
                throw;
            }
        }

        public static (ColorType[,] dummies, ColorType[] busOrder) ProcessLevelJson(LevelJson levelJson)
        {
            // Set the grid data
            var dummies = new ColorType[levelJson.grid_height, levelJson.grid_width];

            int gridIndex = 0;
            for (int i = 0; i < levelJson.grid_height; ++i)
            for (int j = 0; j < levelJson.grid_width; ++j)
            {
                switch (levelJson.grid[gridIndex++])
                {
                    case nameof(JsonGridObjectType.emp):
                        dummies[i, j] = ColorType.Empty;
                        break;
                    case nameof(JsonGridObjectType.r):
                        dummies[i, j] = ColorType.Red;
                        break;
                    case nameof(JsonGridObjectType.g):
                        dummies[i, j] = ColorType.Green;
                        break;
                    case nameof(JsonGridObjectType.b):
                        dummies[i, j] = ColorType.Blue;
                        break;
                    case nameof(JsonGridObjectType.y):
                        dummies[i, j] = ColorType.Yellow;
                        break;
                    default:
                        dummies[i, j] = ColorType.Empty;
                        break;
                }
            }
            
            var busOrder = new ColorType[levelJson.bus_order.Length];
            gridIndex = 0;
            for (int i = 0; i < levelJson.bus_order.Length; ++i)
            {
                switch (levelJson.bus_order[gridIndex++])
                {
                    case nameof(JsonGridObjectType.emp):
                        busOrder[i] = ColorType.Empty;
                        //TODO: Throw exception
                        break;
                    case nameof(JsonGridObjectType.r):
                        busOrder[i] = ColorType.Red;
                        break;
                    case nameof(JsonGridObjectType.g):
                        busOrder[i] = ColorType.Green;
                        break;
                    case nameof(JsonGridObjectType.b):
                        busOrder[i] = ColorType.Blue;
                        break;
                    case nameof(JsonGridObjectType.y):
                        busOrder[i] = ColorType.Yellow;
                        break;
                    default:
                        busOrder[i] = ColorType.Empty;
                        //TODO: Throw exception
                        break;
                }
                
            }

            return (dummies, busOrder);
        }
        
    }
}