using System.Collections.Generic;
using Core.Actors;

namespace MVP.Models.Interface
{
    public interface IBusModel
    {
        public Queue<Bus> BusQueue { get; }

        public Bus ActiveBus { get; }
        public void Initialize(List<Bus> buses);
    }
}