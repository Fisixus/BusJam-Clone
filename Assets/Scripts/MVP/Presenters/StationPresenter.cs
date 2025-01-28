using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Actors;
using Core.Factories.Interface;
using DG.Tweening;
using MVP.Models.Interface;
using MVP.Presenters.Handlers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UTasks;

namespace MVP.Presenters
{
    public class StationPresenter
    {
        private readonly BusSystemHandler _busSystemHandler;
        private readonly GridEscapeHandler _gridEscapeHandler;
        private readonly GoalHandler _goalHandler;
        private readonly IStationModel _stationModel;
        private readonly IBusModel _busModel;
        private readonly IDummyFactory _dummyFactory;


        private Dictionary<Dummy, List<Vector2Int>> _runnableDummies = new();

        public StationPresenter(BusSystemHandler busSystemHandler, GridEscapeHandler gridEscapeHandler, GoalHandler goalHandler,
            IStationModel stationModel, IBusModel busModel, IDummyFactory dummyFactory)
        {
            _busSystemHandler = busSystemHandler;
            _gridEscapeHandler = gridEscapeHandler;
            _goalHandler = goalHandler;
            _stationModel = stationModel;
            _busModel = busModel;
            _dummyFactory = dummyFactory;

            _busSystemHandler.OnMoveDummiesFromWaitingSpots += TryMoveDummiesOnSpotToBus;
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
            _busSystemHandler.OnMoveDummiesFromWaitingSpots -= TryMoveDummiesOnSpotToBus;
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
                touchedDummy.SetOutline(false);
                var tween = touchedDummy.Navigator.MoveAlongPath(worldPositions);
                
                var activeBus = _busModel.ActiveBus;
                
                var chair = activeBus.GetNextAvailableChair();
                chair.IsAvailable = false;
                tween.OnComplete(() =>
                {
                    HandleDummyBoarding(chair, touchedDummy);
                });
            }
            else //Go to waiting spots
            {
                Vector3? waitingSpotPos = AssignToWaitingSpot(touchedDummy);
                if (!waitingSpotPos.HasValue) return;

                worldPositions.Add(waitingSpotPos.Value);
                touchedDummy.SetOutline(false);
                var tween = touchedDummy.Navigator.MoveAlongPath(worldPositions);
                tween.OnComplete(() =>
                {
                    touchedDummy.Navigator.SetAnimationState(DummyAnimations.Idle);
                    touchedDummy.Navigator.ResetRotation();
                    
                    _goalHandler.CheckLevelEndConditions();
                });
            }

            ClearGridPosition(touchedDummy);
            SetAllRunnableDummies();
            HighlightRunnableDummies();
        }

        private void HandleDummyBoarding(BusChair chair, Dummy dummy)
        {
            var activeBus = _busModel.ActiveBus;

            dummy.gameObject.SetActive(false);
            activeBus.SitChair(_dummyFactory.ColorData, chair);
            if (activeBus.AreAllSeatsTaken())
            {
                _goalHandler.DecreaseBusCount();
                UTask.Wait(0.8f).Do(() => _busSystemHandler.MoveBuses());
                //_busSystemHandler.MoveBuses();
            }
        }
        
        private void TryMoveDummiesOnSpotToBus()
        {
            var activeBus = _busModel.ActiveBus;
            if(activeBus == null) return;
            foreach (var spot in _stationModel.BusWaitingSpots)
            {
                if(spot.Dummy is null) continue;//TODO:
                if (!spot.IsAvailable && spot.Dummy.ColorType == activeBus.ColorType)
                {
                    var waitingDummy = spot.Dummy;
                    var doorPos = activeBus.DoorTr.position;
                    doorPos.y = waitingDummy.transform.position.y;
                    List<Vector3> worldPositions = new List<Vector3> { waitingDummy.transform.position, doorPos };
                    var tween = waitingDummy.Navigator.MoveAlongPath(worldPositions);
                    spot.IsAvailable = true;
                    
                    var chair = activeBus.GetNextAvailableChair();
                    chair.IsAvailable = false;
                    
                    tween.OnComplete(() =>
                    {
                        HandleDummyBoarding(chair, waitingDummy);
                        spot.Dummy = null;
                    });
                    
                }
            }
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
            // If the bus still on move
            if (DOTween.IsTweening(activeBus.transform))
            {
                busDoorPos = -Vector3.one;
                return false;
            }
            if (!activeBus.IsBusFull() && activeBus.ColorType == dummy.ColorType)
            {
                busDoorPos = activeBus.DoorTr.position;
                busDoorPos.y = dummy.transform.position.y;
                return true;
            }

            busDoorPos = Vector3.zero;
            return false;
        }

        

        /// <summary>
        /// Assigns the dummy to a waiting line if there is an available spot.
        /// </summary>
        private Vector3? AssignToWaitingSpot(Dummy dummy)
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
