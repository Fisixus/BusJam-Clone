using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Actors;
using Core.Factories.Interface;
using DG.Tweening;
using MVP.Models.Interface;
using UnityEngine;
using UTasks;

namespace MVP.Presenters
{
    public class BusPresenter
    {
        private readonly IBusModel _busModel;
        private readonly IStationModel _stationModel;
        private readonly IBusFactory _busFactory;

        public BusPresenter(IBusModel busModel, IStationModel stationModel, IBusFactory busFactory)
        {
            _busModel = busModel;
            _stationModel = stationModel;
            _busFactory = busFactory;
        }
        
        private void TryMoveDummiesOnStopToBus()
        {
            var activeBus = _busModel.ActiveBus;
            foreach (var spot in _stationModel.BusWaitingSpots)
            {
                if(!spot.IsAvailable)
                Debug.Log(spot.Dummy.ColorType);
                if (!spot.IsAvailable && spot.Dummy.ColorType == activeBus.ColorType)//TODO:
                {
                    var waitingDummy = spot.Dummy;
                    var doorPos = activeBus.DoorTr.position;
                    doorPos.y = waitingDummy.transform.position.y;
                    List<Vector3> worldPositions = new List<Vector3> { waitingDummy.transform.position, doorPos };
                    var tween = waitingDummy.Navigator.MoveAlongPath(worldPositions);
                    spot.IsAvailable = true;
                    tween.OnComplete(() =>
                    {
                        spot.Dummy = null;
                    });
                }
            }
        }

        public void MoveBuses()
        {
            var color = _busFactory.GetNextColor() ?? ColorType.Empty;

            // Get the active bus (front of the queue)
            var activeBus = _busModel.BusQueue.Dequeue();
            _busModel.BusQueue.Enqueue(activeBus); // Put it back in the queue

            List<Bus> buses = new List<Bus>(_busModel.BusQueue);
            _busModel.BusQueue.Clear(); // Temporarily clear queue to avoid modification issues

            MoveActiveBus(activeBus, color);

            for (int i = 0; i < buses.Count - 1; i++)
            {
                MoveBusToNextPosition(buses[i]);
                _busModel.BusQueue.Enqueue(buses[i]); // Re-add the bus after processing
            }
            UTask.Wait(0.6f).Do(()=>
            {
                TryMoveDummiesOnStopToBus();
            });
        }

        /// <summary>
        /// Moves the active bus to the final position, then resets its attributes.
        /// </summary>
        private void MoveActiveBus(Bus activeBus, ColorType color)
        {
            var tween = activeBus.SetPosition(_busFactory.FinalLocationX);
            tween.OnComplete(() =>
            {
                activeBus.BusChairs.ForEach(c => c.ResetChair());
                activeBus.SetPosition(_busFactory.BusData, 0, false);
                activeBus.SetAttributes(0, color);
                activeBus.SetColor(_busFactory.ColorData);
            });
        }

        /// <summary>
        /// Moves a bus to its next position in the queue.
        /// </summary>
        private void MoveBusToNextPosition(Bus bus)
        {
            int newOrder = bus.Order + 1;
            bus.SetAttributes(newOrder, bus.ColorType);
            var animTime = 0.5f;
            bus.SetPosition(_busFactory.BusData, newOrder, true, animTime);
            
        }


        
    }
}
