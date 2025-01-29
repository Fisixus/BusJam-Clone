using System.Collections.Generic;
using Core.Actors;
using Core.Actors.Data;
using Core.Factories.Interface;
using Core.Factories.Pools;
using UnityEngine;

namespace Core.Factories
{
    public class BusWaitingSpotFactory : ObjectFactory<BusWaitingSpot>, IBusWaitingSpotFactory
    {
        [field:SerializeField]
        public Vector2 Spacing { get; private set; } = new Vector2(0.8f, 0f);
        
        private List<BusWaitingSpot> _allSpots = new();

        
        public override void PreInitialize()
        {
            Pool = new ObjectPool<BusWaitingSpot>(ObjPrefab, ParentTr, 5);
            _allSpots = new List<BusWaitingSpot>(5);
        }
        
        public void PopulateSpots(List<BusWaitingSpot> spots)
        {
            var columns = 5; //TODO: actually 7 first and last one will be locked
            var startX = -((columns - 1) * Spacing.x) / 2;
            for (int i = 0; i < columns; i++)
            {
                var spot = CreateObj();
                spot.Order = i;
                spot.SetStartPosition(startX, Spacing);
                if (spot != null)
                {
                    spots.Add(spot);
                }
            }
        }
        
        public override BusWaitingSpot CreateObj()
        {
            var item = base.CreateObj();
            _allSpots.Add(item);
            return item;
        }

        public override void DestroyObj(BusWaitingSpot emptyItem)
        {
            base.DestroyObj(emptyItem);
            //emptyItem.SetAttributes(-Vector2Int.one, ColorType.None);
            _allSpots.Remove(emptyItem);
        }

        public void DestroyAllWaitingSpots()
        {
            var itemsToDestroy = new List<BusWaitingSpot>(_allSpots);
            base.DestroyObjs(itemsToDestroy);
            _allSpots.Clear();
        }
        
    }
}
