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
        
        public override void PreInitialize()
        {
            Pool = new ObjectPool<BusWaitingSpot>(ObjPrefab, ParentTr, 5);
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
        
    }
}
