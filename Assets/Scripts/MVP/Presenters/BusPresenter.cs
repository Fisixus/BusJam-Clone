using Core.Factories.Interface;
using DG.Tweening;
using MVP.Models.Interface;

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
            var chairs = _busModel.ActiveBus.BusChairs;
            var color = _busModel.ActiveBus.ColorType;
            foreach (var chair in chairs)
            {
                if (chair.IsAvailable)
                {
                    chair.IsAvailable = false;
                    chair.SetChairOwner(color, _dummyFactory.ColorData);
                    break;
                }
            }
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
                    _busModel.BusQueue.Enqueue(bus);
                }
                else
                {
                    //TODO: Stop other buses
                    //throw new Exception("Bus count is 0");
                }
                
            }
        }
        
    }
}
