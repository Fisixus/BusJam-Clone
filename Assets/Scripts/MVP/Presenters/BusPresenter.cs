using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Actors;
using Core.Factories.Interface;
using DG.Tweening;
using MVP.Models.Interface;
using UnityEngine;

namespace MVP.Presenters
{
    public class BusPresenter
    {
        private readonly IBusModel _busModel;
        private readonly IBusFactory _busFactory;
        private readonly IDummyFactory _dummyFactory;

        public BusPresenter(IBusModel busModel, IBusFactory busFactory, IDummyFactory dummyFactory)
        {
            _busModel = busModel;
            _busFactory = busFactory;
            _dummyFactory = dummyFactory;
        }

        public void SitNextChair()
        {
            var activeBus = _busModel.ActiveBus;
            var color = activeBus.ColorType;

            BusChair nextChair = GetNextAvailableChair(activeBus);
            if (nextChair != null)
            {
                nextChair.IsAvailable = false;
                nextChair.SetChairOwner(color, _dummyFactory.ColorData);
            }

            if (IsBusFull(activeBus))
            {
                MoveBuses();
            }
        }
        
        /// <summary>
        /// Checks if the given bus is full.
        /// </summary>
        public bool IsBusFull(Bus bus)
        {
            return bus.BusChairs.All(chair => !chair.IsAvailable);
        }

        /// <summary>
        /// Finds the next available chair in the active bus.
        /// </summary>
        private BusChair GetNextAvailableChair(Bus bus)
        {
            return bus.BusChairs.FirstOrDefault(chair => chair.IsAvailable);
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
            bus.SetPosition(_busFactory.BusData, newOrder, true);
            bus.SetAttributes(newOrder, bus.ColorType);
        }


        
    }
}
