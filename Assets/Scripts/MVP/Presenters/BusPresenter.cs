using System;
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
        private readonly IBusFactory _busFactory;

        public BusPresenter(IBusModel busModel, IBusFactory busFactory)
        {
            _busModel = busModel;
            _busFactory = busFactory;
        }

        public void SitNextChair()
        {
            
        }

        public bool IsBusFull()
        {
            var activeBus = _busModel.ActiveBus;
            foreach (var chair in activeBus.BusChairs)
            {
                if (chair.IsAvailable)
                    return false;
            }

            return true;
        }

        public void MoveBuses()
        {
            for (int i = 0; i < _busModel.BusQueue.Count; i++)
            {
                var bus = _busModel.BusQueue.Dequeue();
                var color = _busFactory.GetNextColor();
                if (color != null)
                {
                    var newOrder = (i + 1) % _busModel.BusQueue.Count;
                    if (newOrder < bus.Order)
                    {
                        //TODO: Move bus to final location then teleport to newOrder location
                        var tween = bus.SetPosition(_busFactory.FinalLocationX);
                        tween.OnComplete(() =>
                        {
                            bus.SetPosition(_busFactory.BusData, newOrder, true);
                            bus.SetColor(_busFactory.ColorData);
                        });
                    }
                    else
                    {
                        bus.SetPosition(_busFactory.BusData, newOrder, true);
                        bus.SetColor(_busFactory.ColorData);
                    }
                }
                else
                {
                    //TODO: Stop other buses
                    //throw new Exception("Bus count is 0");
                }
                
                _busModel.BusQueue.Enqueue(bus);
            }
        }
        
    }
}
