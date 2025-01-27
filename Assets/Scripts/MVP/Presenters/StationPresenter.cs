using System.Collections.Generic;
using Core;
using Core.Actors;
using MVP.Models.Interface;
using MVP.Presenters.Handlers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVP.Presenters
{
    public class StationPresenter
    {
        private readonly GridEscapeHandler _gridEscapeHandler;
        private readonly IStationModel _stationModel;

        private Dictionary<Dummy, List<Vector2Int>> _runnableDummies = new();

        public StationPresenter(GridEscapeHandler gridEscapeHandler, IStationModel stationModel)
        {
            _gridEscapeHandler = gridEscapeHandler;
            _stationModel = stationModel;
            
            UserInput.OnDummyTouched += OnTouch;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            Dispose();
        }

        private void Dispose()
        {
            // Unsubscribe from static and instance events
            UserInput.OnDummyTouched -= OnTouch;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
        
        private void OnTouch(Dummy touchedDummy)
        {
            // Check if the touched dummy can escape
            if (_runnableDummies.TryGetValue(touchedDummy, out var path) && path != null)
            {
                // Update grid position to empty
                var coord = touchedDummy.Coordinate;
                _stationModel.Dummies[coord.x, coord.y].ColorType = ColorType.Empty;
                
                // Transform path coords to world pos
                List<Vector3> worldPositions = new List<Vector3>();
                foreach (var p in path)
                {
                    //Debug.Log("path:" + p);
                    var worldPos = _stationModel.Grid[p.x, p.y].transform.position;
                    worldPos.y = touchedDummy.transform.position.y;
                    worldPositions.Add(worldPos);
                }
                
                // TODO: After dummy reaches escape position, move him either through bus or waiting line
                foreach (var busWaitingSpot in _stationModel.BusWaitingSpots)
                {
                    if (busWaitingSpot.IsAvailable)
                    {
                        busWaitingSpot.IsAvailable = false;
                        busWaitingSpot.Dummy = touchedDummy;
                        var worldPos = busWaitingSpot.transform.position;
                        worldPos.y = touchedDummy.transform.position.y;
                        worldPositions.Add(worldPos);
                        break;
                    }
                }                
                touchedDummy.Navigator.MoveAlongPath(worldPositions);
            }
            else
            {
                // Show angry person emoji
                touchedDummy.PlayEmojiAnimation();
                return;
            }

            // Highlight runnable dummies after touch interaction
            SetAllRunnableDummies();
            HighlightRunnableDummies();
        }

        public void SetAllRunnableDummies()
        {
            _runnableDummies = _gridEscapeHandler.GetAllEscapePaths();
        }

        public void HighlightRunnableDummies()
        {
            foreach (var kv in _runnableDummies)
            {
                kv.Key.SetOutline(true);
            }
        }


    }
}
