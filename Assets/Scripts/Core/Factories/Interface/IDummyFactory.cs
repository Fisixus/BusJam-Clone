using System.Collections.Generic;
using Core.Actors;
using Core.Actors.Data;
using UnityEngine;

namespace Core.Factories.Interface
{
    public interface IDummyFactory : IFactory<Dummy>
    {
        public ColorDataSO ColorData { get; }
        public Vector2 Spacing { get; }
        public void PopulateDummies(ColorType[,] colorTypes, List<Dummy> dummies);
        public void DestroyAllDummies();
    }
}