using System.Collections.Generic;
using Core.Actors;
using UnityEngine;

namespace Core.Factories.Interface
{
    public interface IBusWaitingSpotFactory:IFactory<BusWaitingSpot>
    {
        public Vector2 Spacing { get; }
        public void PopulateSpots(List<BusWaitingSpot> spots);
        public void DestroyAllWaitingSpots();
    }
}
