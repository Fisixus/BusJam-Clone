using System.Collections.Generic;
using Core.Actors;

namespace Core.Factories.Interface
{
    public interface IBusFactory
    {
        public void PopulateBuses(ColorType[] colorTypes, List<Bus> buses);
    }
}
