using System.Collections.Generic;
using Core.Actors;
using MVP.Models.Interface;

namespace MVP.Models
{
    public class BusModel : IBusModel
    {
        public Queue<Bus> BusQueue { get; private set; } = new Queue<Bus>();
        
        private Bus _activeBus;

        public Bus ActiveBus => BusQueue.Peek();

        public void Initialize(List<Bus> buses)
        {
            foreach (var bus in buses)
            {
                BusQueue.Enqueue(bus);
            }
        }
    }
}
