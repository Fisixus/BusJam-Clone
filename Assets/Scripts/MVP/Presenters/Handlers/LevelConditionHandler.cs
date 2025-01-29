using System;
using System.Linq;
using System.Text;
using MVP.Helpers;
using MVP.Models.Interface;
using MVP.Views.Interface;
using UnityEngine;
using UTasks;

namespace MVP.Presenters.Handlers
{
    public class LevelConditionHandler
    {
        private readonly ILevelUIView _levelUIView;
        private readonly IStationModel _stationModel;

        public event Action OnLevelCompleted;
        public event Action OnLevelFailed;

        private float _timeLimit;
        private int _busCount;
        private bool _isLevelCompleted;

        private float _timeLeft;
        private StringBuilder _timeStringBuilder = new StringBuilder();
        private bool _isRunning = false;
        private UTask _taskTimer;

        public LevelConditionHandler(ILevelUIView levelUIView, IStationModel stationModel)
        {
            _levelUIView = levelUIView;
            _stationModel = stationModel;
        }

        public void Initialize(int busCount, float levelTimeInSeconds)
        {
            // Clone the provided goals into the internal goals array
            _busCount = busCount;
            _timeLimit = levelTimeInSeconds;
            _timeLeft = _timeLimit;
            _isLevelCompleted = false;

            _taskTimer?.Kill();
            StartTimer();
        }

        public void DecreaseBusCount()
        {
            _busCount--;

            if (AreAllBusesGone() && !_isLevelCompleted)
            {
                HandleLevelSuccess();
            }
            //CheckLevelEndConditions();
        }

        private void StartTimer()
        {
            if (_isRunning) return; // Prevent duplicate coroutines
            _taskTimer = UTask.For(_timeLimit).Do(() =>
            {
                _timeLeft -= Time.deltaTime;
                _levelUIView.TimerText.text = TimerHelper.FormatTime(_timeStringBuilder, (int)_timeLeft);
                if (_timeLeft <= 0 && !_isLevelCompleted)
                {
                    HandleLevelFailure();
                }
            });
        }

        public void HandleLevelFailure()
        {
            if (_isLevelCompleted) return;

            OnLevelFailed?.Invoke();
            _isLevelCompleted = true;
        }

        public void HandleLevelSuccess()
        {
            if (_isLevelCompleted) return;

            OnLevelCompleted?.Invoke();
            _isLevelCompleted = true;
        }

        private bool AreAllBusesGone()
        {
            return _busCount <= 0;
        }

        /// <summary>
        /// Checks if all waiting spots are occupied.
        /// </summary>
        public bool AreAllWaitingSpotsFull()
        {
            return _stationModel.BusWaitingSpots.All(spot => !spot.IsAvailable);
        }
    }
}