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
                        dummies[i, j] = ColorType.None;
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
                        dummies[i, j] = ColorType.None;
                        break;
                }
            }
            
            var busOrder = new ColorType[levelJson.bus_count];
            gridIndex = 0;
            for (int i = 0; i < levelJson.bus_order.Length; ++i)
            {
                switch (levelJson.bus_order[gridIndex++])
                {
                    case nameof(JsonGridObjectType.emp):
                        busOrder[i] = ColorType.None;
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
                        busOrder[i] = ColorType.None;
                        //TODO: Throw exception
                        break;
                }
                
            }

            return (dummies, busOrder);
        }
        
        public static JsonGridObjectType[,] ConvertGridDummiesToJsonObjectType(ColorType[,] gridObjectTypes)
        {
            JsonGridObjectType[,] jsonEnumResponses =
                new JsonGridObjectType[gridObjectTypes.GetLength(0), gridObjectTypes.GetLength(1)];
            for (int i = 0; i < gridObjectTypes.GetLength(0); ++i)
            {
                for (int j = 0; j < gridObjectTypes.GetLength(1); ++j)
                {
                    switch (gridObjectTypes[i, j])
                    {
                        case ColorType.Red:
                            jsonEnumResponses[i, j] = JsonGridObjectType.r;
                            break;
                        case ColorType.Green:
                            jsonEnumResponses[i, j] = JsonGridObjectType.g;
                            break;
                        case ColorType.Blue:
                            jsonEnumResponses[i, j] = JsonGridObjectType.b;
                            break;
                        case ColorType.Yellow:
                            jsonEnumResponses[i, j] = JsonGridObjectType.y;
                            break;
                        default:
                            jsonEnumResponses[i, j] = JsonGridObjectType.emp;
                            break;
                    }
                }
            }

            return jsonEnumResponses;
        }
        
        public static JsonGridObjectType[] ConvertGridBusesToJsonObjectType(ColorType[] levelInfoBuses)
        {
            JsonGridObjectType[] jsonEnumResponses =
                new JsonGridObjectType[levelInfoBuses.Length];
            for (int j = 0; j < levelInfoBuses.Length; ++j)
            {
                switch (levelInfoBuses[j])
                {
                    case ColorType.Red:
                        jsonEnumResponses[j] = JsonGridObjectType.r;
                        break;
                    case ColorType.Green:
                        jsonEnumResponses[j] = JsonGridObjectType.g;
                        break;
                    case ColorType.Blue:
                        jsonEnumResponses[j] = JsonGridObjectType.b;
                        break;
                    case ColorType.Yellow:
                        jsonEnumResponses[j] = JsonGridObjectType.y;
                        break;
                    default:
                        jsonEnumResponses[j] = JsonGridObjectType.emp;
                        break;
                }
            }
            return jsonEnumResponses;
        }
        
        public static LevelJson ConvertToLevelJson(int gridWidth, int gridHeight, int timer, int busCount, JsonGridObjectType[] buses,
            JsonGridObjectType[,] dummies)
        {
            LevelJson levelJson = new LevelJson
            {
                level_number = 0,
                grid_width = gridWidth,
                grid_height = gridHeight,
                bus_count = busCount,
                bus_order = new string[busCount],
                timer = timer,
                grid = new string[gridWidth * gridHeight]
            };

            for (int x = 0; x < gridHeight; x++)
            {
                for (int y = 0; y < gridWidth; y++)
                {
                    int index = (x * gridWidth) + y; // Correct indexing from 0 to (gridHeight * gridWidth - 1)
                    levelJson.grid[index] = dummies[x, y].ToString();
                }
            }
            for (int x = 0; x < busCount; x++)
            {
                    // Reverse the row order but keep elements in the same order within each row
                    //int reversedRow = gridHeight - 1 - x;
                    levelJson.bus_order[x] = buses[x].ToString();
            }

            return levelJson;
        }


        
    }
}