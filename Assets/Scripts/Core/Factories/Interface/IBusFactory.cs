using System.Collections.Generic;
using Core.Actors;
using Core.Actors.Data;

namespace Core.Factories.Interface
{
    public interface IBusFactory : IFactory<Bus>
    {
        public float FinalLocationX { get; }
        public BusDataSO BusData { get; }
        public ColorDataSO ColorData { get; }
        public void PopulateBuses(ColorType[] colorTypes, List<Bus> buses);
        public ColorType? GetNextColor();
        public void DestroyAllBuses();
    }
}