using System.Collections.Generic;
using Core.Actors;
using Core.Factories.Interface;
using Core.Factories.Pools;

namespace Core.Factories
{
    public class BusFactory : ObjectFactory<Bus>, IBusFactory
    {
        public override void PreInitialize()
        {
            //Pool = new ObjectPool<Bus>(ObjPrefab, ParentTr, 4);
        }

        public void PopulateBuses(ColorType[] colorTypes, List<Bus> buses)
        {
            //TODO:
        }
    }
}
