using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Actors;
using MVP.Helpers;
using MVP.Models.Interface;
using MVP.Views.Interface;
using UnityEngine;
using UTasks;

namespace MVP.Presenters.Handlers
{
    public class GoalHandler
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

        public GoalHandler(ILevelUIView levelUIView, IStationModel stationModel)
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
            
            StartTimer();
        }

        public void DecreaseBusCount()
        {
            _busCount--;
            CheckLevelEndConditions();
        }
        
        private void StartTimer()
        {
            if (_isRunning) return; // Prevent duplicate coroutines
            UTask.For(_timeLimit).Do(() =>
            {
                _timeLeft -= Time.deltaTime;
                _levelUIView.TimerText.text = TimerHelper.FormatTime(_timeStringBuilder, (int)_timeLeft);
                if(_timeLeft <= 0)
                    CheckLevelEndConditions();
            });
        }
        public void CheckLevelEndConditions()
        {
            if (_isLevelCompleted) return;
            if (AreAllBusesGone())
            {
                OnLevelCompleted?.Invoke();
                _isLevelCompleted = true;
            }
            else if (IsTimeUp() || AreAllWaitingSpotsFull())
            {
                OnLevelFailed?.Invoke();
                _isLevelCompleted = true;
            }
        }
        private bool AreAllBusesGone()
        {
            return _busCount <= 0;
        }
        private bool IsTimeUp()
        {
            return _timeLeft <= 0f;
        }
        
        /// <summary>
        /// Checks if all waiting spots are occupied.
        /// </summary>
        private bool AreAllWaitingSpotsFull()
        {
            return _stationModel.BusWaitingSpots.All(spot => !spot.IsAvailable);
        }
        
    }
}
