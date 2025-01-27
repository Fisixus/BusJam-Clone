using System.Collections.Generic;
using Core;
using Core.Actors;
using DG.Tweening;
using MVP.Models.Interface;
using MVP.Presenters.Handlers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVP.Presenters
{
    public class StationPresenter
    {
        private readonly BusPresenter _busPresenter;
        private readonly GridEscapeHandler _gridEscapeHandler;
        private readonly IStationModel _stationModel;
        private readonly IBusModel _busModel;

        private Dictionary<Dummy, List<Vector2Int>> _runnableDummies = new();

        public StationPresenter(BusPresenter busPresenter, GridEscapeHandler gridEscapeHandler, IStationModel stationModel, IBusModel busModel)
        {
            _busPresenter = busPresenter;
            _gridEscapeHandler = gridEscapeHandler;
            _stationModel = stationModel;
            _busModel = busModel;
            
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
            if (!_runnableDummies.TryGetValue(touchedDummy, out var path) || path == null)
            {
                touchedDummy.PlayEmojiAnimation(); // Show angry person emoji
                return;
            }

            List<Vector3> worldPositions = ConvertPathToWorldPositions(path, touchedDummy);

            if (CanBoardBus(touchedDummy, out Vector3 busDoorPos))
            {
                worldPositions.Add(busDoorPos);
                var tween = touchedDummy.Navigator.MoveAlongPath(worldPositions);
                tween.OnComplete(() =>
                {
                    _busPresenter.SitNextChair();
                    touchedDummy.gameObject.SetActive(false);
                });
            }
            else
            {
                Vector3? waitingSpotPos = AssignToWaitingLine(touchedDummy);
                if (waitingSpotPos.HasValue)
                {
                    worldPositions.Add(waitingSpotPos.Value);
                }
                touchedDummy.SetOutline(false);
                var tween = touchedDummy.Navigator.MoveAlongPath(worldPositions);
                tween.OnComplete(() =>
                {
                    touchedDummy.Navigator.SetAnimationState(DummyAnimations.Idle);
                    touchedDummy.Navigator.ResetRotation();
                });
            }

            ClearGridPosition(touchedDummy);
            SetAllRunnableDummies();
            HighlightRunnableDummies();
        }

        /// <summary>
        /// Converts grid path coordinates to world positions.
        /// </summary>
        private List<Vector3> ConvertPathToWorldPositions(List<Vector2Int> path, Dummy dummy)
        {
            List<Vector3> worldPositions = new List<Vector3>();

            foreach (var p in path)
            {
                Vector3 worldPos = _stationModel.Grid[p.x, p.y].transform.position;
                worldPos.y = dummy.transform.position.y;
                worldPositions.Add(worldPos);
            }

            return worldPositions;
        }

        /// <summary>
        /// Checks if the dummy can board the bus and returns the bus door position.
        /// </summary>
        private bool CanBoardBus(Dummy dummy, out Vector3 busDoorPos)
        {
            var activeBus = _busModel.ActiveBus;
            if (!_busPresenter.IsBusFull(_busModel.ActiveBus) && activeBus.ColorType == dummy.ColorType)
            {
                busDoorPos = activeBus.DoorTr.position;
                busDoorPos.y = dummy.transform.position.y;
                return true;
            }

            busDoorPos = Vector3.zero;
            return false;
        }

        /// <summary>
        /// Assigns the dummy to a waiting line if the bus is full.
        /// </summary>
        private Vector3? AssignToWaitingLine(Dummy dummy)
        {
            foreach (var busWaitingSpot in _stationModel.BusWaitingSpots)
            {
                if (!busWaitingSpot.IsAvailable) continue;

                busWaitingSpot.IsAvailable = false;
                busWaitingSpot.Dummy = dummy;
                
                Vector3 worldPos = busWaitingSpot.transform.position;
                worldPos.y = dummy.transform.position.y;
                return worldPos;
            }

            return null;
        }

        /// <summary>
        /// Clears the dummy's position in the grid.
        /// </summary>
        private void ClearGridPosition(Dummy dummy)
        {
            var coord = dummy.Coordinate;
            _stationModel.Dummies[coord.x, coord.y].ResetAttributes();
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
