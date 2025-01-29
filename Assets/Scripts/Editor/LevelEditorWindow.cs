using System.IO;
using Core.LevelSerialization;
using DI.Contexts;
using MVP.Helpers;
using MVP.Models.Interface;
using MVP.Presenters;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class LevelEditorWindow : EditorWindow
    {
        private int _levelNumber = 0;
        private int _gridWidth = 5;
        private int _gridHeight = 5;

        private int _timer = 60;

        private JsonGridObjectType[,] _gridDummies;

        private int _busCount = 3;
        private JsonGridObjectType[] _gridBuses;

        [MenuItem("Tools/Level Editor")]
        public static void ShowWindow()
        {
            GetWindow<LevelEditorWindow>("Level Editor");
        }

        private void OnEnable()
        {
            InitializeGrid();
        }

        private void OnGUI()
        {
            DrawHeader();
            DrawLevelControls();
            DrawDummyGridControls();
            DrawDummyGridTable();
            DrawGridBuses();
            DrawApplyChangesAndSaveButton();
        }

        #region Header and Controls

        private void DrawHeader()
        {
            GUILayout.Label("Level Editor", EditorStyles.boldLabel);
        }

        private void DrawLevelControls()
        {
            _levelNumber = EditorGUILayout.IntField("Level Number", _levelNumber);

            if (GUILayout.Button("Load Level"))
            {
                LoadLevel();
            }
        }

        private void DrawDummyGridControls()
        {
            int newGridWidth = EditorGUILayout.IntField("Grid Width", _gridWidth);
            int newGridHeight = EditorGUILayout.IntField("Grid Height", _gridHeight);
            _timer = EditorGUILayout.IntField("Timer", _timer);
            _busCount = EditorGUILayout.IntField("Bus Count", _busCount);

            if (newGridWidth != _gridWidth || newGridHeight != _gridHeight)
            {
                ResizeGrid(newGridHeight, newGridWidth);
                _gridWidth = newGridWidth;
                _gridHeight = newGridHeight;
            }

            if (_gridBuses == null || _gridBuses.Length != _busCount)
            {
                ResizeBusArray(_busCount);
            }
        }

        private void DrawDummyGridTable()
        {
            EditorGUILayout.LabelField("Dummies Table", EditorStyles.boldLabel);

            if (_gridDummies == null)
            {
                Debug.LogWarning("Dummies not initialized. Initializing with default values.");
                _gridDummies = new JsonGridObjectType[_gridHeight, _gridWidth];
            }

            for (int i = 0; i < _gridHeight; i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < _gridWidth; j++)
                {
                    _gridDummies[i, j] = (JsonGridObjectType)EditorGUILayout.EnumPopup(_gridDummies[i, j]);
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawGridBuses()
        {
            EditorGUILayout.LabelField("Grid Buses", EditorStyles.boldLabel);

            if (_gridBuses == null)
            {
                Debug.LogWarning("Buses not initialized. Initializing with default values.");
                _gridBuses = new JsonGridObjectType[_busCount];
            }

            for (int i = 0; i < _gridBuses.Length; i++)
            {
                _gridBuses[i] = (JsonGridObjectType)EditorGUILayout.EnumPopup($"Bus {i}", _gridBuses[i]);
            }
        }


        private void DrawApplyChangesAndSaveButton()
        {
            if (GUILayout.Button("Apply Changes"))
            {
                ApplyChanges();
            }

            if (GUILayout.Button("Save Level"))
            {
                SaveLevel();
            }
        }

        #endregion

        #region Level Logic

        private void InitializeGrid()
        {
            if (_gridDummies == null)
            {
                _gridDummies = new JsonGridObjectType[_gridHeight, _gridWidth];
            }
        }

        private void LoadLevel()
        {
            if (!IsLevelSceneActive())
            {
                Debug.LogWarning("You are not in the LevelScene!");
                return;
            }

            var levelModel = ProjectContext.Container.Resolve<ILevelModel>();
            var levelInfo = levelModel.LoadLevel(_levelNumber);

            if (levelInfo == null)
            {
                Debug.LogError($"Failed to load level {_levelNumber}.");
                return;
            }

            // Update grid dimensions and move count
            _gridWidth = levelInfo.DummyGridSize.y;
            _gridHeight = levelInfo.DummyGridSize.x;
            _timer = levelInfo.Timer;
            _busCount = levelInfo.BusCount;

            // Convert grid data to JsonGridObjectType and update _gridObjects
            var gridData = LevelSerializer.ConvertGridDummiesToJsonObjectType(levelInfo.Dummies);
            ResizeGrid(_gridHeight, _gridWidth);
            PopulateGrid(gridData);

            var busData = LevelSerializer.ConvertGridBusesToJsonObjectType(levelInfo.Buses);
            ResizeBusArray(_busCount);
            PopulateBuses(busData);

            Debug.Log($"Level {_levelNumber} loaded successfully.");
        }

        private void ApplyChanges()
        {
            var levelJson = LevelSerializer.ConvertToLevelJson(_gridWidth, _gridHeight, _timer, _busCount, _gridBuses,
                _gridDummies);
            var (gridObjectTypes, levelGoals) = LevelSerializer.ProcessLevelJson(levelJson);

            var levelInfo = new LevelInfo(levelJson.level_number, gridObjectTypes, levelGoals, levelJson.timer);
            Debug.Log($"Level JSON created:\n{JsonUtility.ToJson(levelJson, true)}");

            CreateLevel(levelInfo);
        }

        private static void CreateLevel(LevelInfo levelInfo)
        {
            if (!IsLevelSceneActive())
            {
                Debug.LogWarning("You are not in the LevelScene!");
                return;
            }

            var context = SceneHelper.FindSceneContextInActiveScene();
            var levelPresenter = context.SceneContainer.Resolve<LevelPresenter>();
            levelPresenter.LoadFromLevelEditor(levelInfo);

            Debug.Log("Level created successfully.");
        }

        private void SaveLevel()
        {
            var levelJson = LevelSerializer.ConvertToLevelJson(_gridWidth, _gridHeight, _timer, _busCount, _gridBuses,
                _gridDummies);
            string levelData = JsonUtility.ToJson(levelJson, true);

            // Ensure the Levels folder exists
            string folderPath = "Assets/Resources/Levels";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Format level name as level_00, level_01, etc.
            string levelFileName = $"level_{_levelNumber:D2}.json";
            string fullPath = Path.Combine(folderPath, levelFileName);

            // Save the JSON data to a file
            File.WriteAllText(fullPath, levelData);
            AssetDatabase.Refresh(); // Refresh Unity Asset Database to recognize the new file

            Debug.Log($"Level {_levelNumber} saved successfully at {fullPath}");
        }

        #endregion

        #region Grid Management

        private void ResizeGrid(int newHeight, int newWidth)
        {
            if (newHeight <= 0 || newWidth <= 0)
            {
                Debug.LogError("Invalid grid dimensions. Height and Width must be greater than 0.");
                return;
            }

            var newGrid = new JsonGridObjectType[newHeight, newWidth];

            if (_gridDummies != null)
            {
                for (int i = 0; i < Mathf.Min(_gridHeight, newHeight); i++)
                {
                    for (int j = 0; j < Mathf.Min(_gridWidth, newWidth); j++)
                    {
                        // Ensure no out-of-bounds access
                        if (i < _gridDummies.GetLength(0) && j < _gridDummies.GetLength(1))
                        {
                            newGrid[i, j] = _gridDummies[i, j];
                        }
                    }
                }
            }

            _gridDummies = newGrid;
            _gridHeight = newHeight;
            _gridWidth = newWidth;
        }

        private void ResizeBusArray(int newSize)
        {
            if (newSize <= 0)
            {
                Debug.LogError("Invalid bus count. Must be greater than 0.");
                return;
            }

            var newBuses = new JsonGridObjectType[newSize];
            if (_gridBuses != null)
            {
                for (int i = 0; i < Mathf.Min(_gridBuses.Length, newSize); i++)
                {
                    if (i < _gridBuses.Length)
                        newBuses[i] = _gridBuses[i];
                }
            }

            _gridBuses = newBuses;
        }

        private void PopulateGrid(JsonGridObjectType[,] gridData)
        {
            for (int i = 0; i < _gridHeight; i++)
            {
                for (int j = 0; j < _gridWidth; j++)
                {
                    _gridDummies[i, j] = gridData[i, j];
                }
            }
        }

        private void PopulateBuses(JsonGridObjectType[] busData)
        {
            for (int i = 0; i < _busCount; i++)
            {
                _gridBuses[i] = busData[i];
            }
        }

        #endregion

        #region Utility Methods

        private static bool IsLevelSceneActive()
        {
            return SceneManager.GetActiveScene().name == "LevelScene";
        }

        #endregion
    }
}