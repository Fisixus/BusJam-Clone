using System;
using System.Collections.Generic;
using Core;
using Core.Actors;
using Core.Factories.Interface;
using DG.Tweening;
using MVP.Models.Interface;
using UnityEngine;
using UTasks;

namespace MVP.Presenters.Handlers
{
    public class BusSystemHandler
    {
        private readonly IBusModel _busModel;
        private readonly IBusFactory _busFactory;
        
        public event Action OnMoveDummiesFromWaitingSpots;
        
        public BusSystemHandler(IBusModel busModel, IBusFactory busFactory)
        {
            _busModel = busModel;
            _busFactory = busFactory;
        }
        
        public void MoveBuses()
        {
            var color = _busFactory.GetNextColor() ?? ColorType.None;
            if(_busModel.BusQueue.Count == 0) return;
            // Get the active bus (front of the queue)
            var activeBus = _busModel.BusQueue.Dequeue();
            List<Bus> buses = new List<Bus>(_busModel.BusQueue);
            _busModel.BusQueue.Clear(); // Temporarily clear queue to avoid modification issues
            MoveActiveBus(activeBus, color);

            var animTime = 0.8f;
            for (int i = 0; i < buses.Count; i++)
            {
                MoveBusToNextPosition(buses[i], animTime);
                _busModel.BusQueue.Enqueue(buses[i]); // Re-add the bus after processing
            }
            
            _busModel.BusQueue.Enqueue(activeBus); // Put it back in the queue

            UTask.Wait(animTime).Do(()=>
            {
                OnMoveDummiesFromWaitingSpots?.Invoke();
                //TryMoveDummiesOnStopToBus();
            });
        }

        /// <summary>
        /// Moves the active bus to the final position, then resets its attributes.
        /// </summary>
        private void MoveActiveBus(Bus activeBus, ColorType color)
        {
            if(activeBus == null) return;
            var tween = activeBus.SetPosition(_busFactory.FinalLocationX);
            tween.OnComplete(() =>
            {
                activeBus.BusChairs.ForEach(c => c.ResetChair());
                activeBus.SetPosition(_busFactory.BusData, 0, false);
                activeBus.SetAttributes(0, color);
                activeBus.SetColor(_busFactory.ColorData);
                if(color == ColorType.None) activeBus.gameObject.SetActive(false);
            });
        }

        /// <summary>
        /// Moves a bus to its next position in the queue.
        /// </summary>
        private void MoveBusToNextPosition(Bus bus, float animTime)
        {
            int newOrder = bus.Order + 1;
            bus.SetAttributes(newOrder, bus.ColorType);
            bus.SetPosition(_busFactory.BusData, newOrder, true, animTime);
            
        }
    }
}
