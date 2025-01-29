using System.Collections.Generic;
using Core.Actors;
using MVP.Models.Interface;

namespace MVP.Models
{
    public class BusModel : IBusModel
    {
        public Queue<Bus> BusQueue { get; private set; } = new Queue<Bus>();
        
        private Bus _activeBus;

        public Bus ActiveBus
        {
            get
            {
                if(BusQueue.Count > 0)
                    return BusQueue.Peek();
                return null;
            }
        }
        
        public void Initialize(List<Bus> buses)
        {
            BusQueue = new Queue<Bus>();
            foreach (var bus in buses)
            {
                BusQueue.Enqueue(bus);
            }
        }
    }
}
