using System.Collections.Generic;
using Core.Actors;
using Core.Actors.Data;
using Core.Factories.Interface;
using Core.Factories.Pools;
using UnityEngine;

namespace Core.Factories
{
    public class BusFactory : ObjectFactory<Bus>, IBusFactory
    {
        [field: SerializeField]
        public float FinalLocationX { get; private set; }
        
        [field: SerializeField]
        public ColorDataSO ColorData { get; private set; }
        [field: SerializeField]
        public BusDataSO BusData { get; private set; }
        
        private List<Bus> _allBuses = new();
        private Queue<ColorType> _busColors = new();
        
        private const int BusPoolCount = 3;

        public override void PreInitialize()
        {
            Pool = new ObjectPool<Bus>(ObjPrefab, ParentTr, BusPoolCount);
            _allBuses = new List<Bus>(BusPoolCount);
        }

        public void PopulateBuses(ColorType[] colorTypes, List<Bus> buses)
        {
            foreach (var colorType in colorTypes)
            {
                _busColors.Enqueue(colorType);
            }
            
            for (int i = Mathf.Min(BusPoolCount-1, colorTypes.Length-1); i >= 0; i--)
            {
                var color = _busColors.Dequeue();
                var bus = GenerateBus(color, i);
                buses.Add(bus);
            }
        }

        public ColorType? GetNextColor()
        {
            if (_busColors.Count == 0) return null;
            return _busColors.Dequeue();
        }
        
        private Bus GenerateBus(ColorType colorType, int order)
        {
            var bus = CreateObj();
            bus.SetPosition(BusData, order);
            bus.SetAttributes(order, colorType);
            bus.SetColor(ColorData);
            return bus;
        }
        
        public override Bus CreateObj()
        {
            var bus = base.CreateObj();
            _allBuses.Add(bus);
            return bus;
        }

        public override void DestroyObj(Bus emptyBus)
        {
            base.DestroyObj(emptyBus);
            emptyBus.SetAttributes(-1, ColorType.Empty);
            _allBuses.Remove(emptyBus);
        }

        public void DestroyAllBuses()
        {
            var busesToDestroy = new List<Bus>(_allBuses);
            base.DestroyObjs(busesToDestroy);
            _allBuses.Clear();
        }
    }
}
