using System.Collections.Generic;
using Core.Actors;

namespace Core.Factories.Interface
{
    public interface IDummyFactory : IFactory<Dummy>
    {
        public void PopulateDummies(ColorType[,] colorTypes, List<Dummy> dummies);
    }
}
