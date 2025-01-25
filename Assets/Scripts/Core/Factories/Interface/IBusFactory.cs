using System.Collections.Generic;
using Core.Actors;

namespace Core.Factories.Interface
{
    public interface IBusFactory:IFactory<Bus>
    {
        public void PopulateBuses(ColorType[] colorTypes, List<Bus> buses);
    }
}
