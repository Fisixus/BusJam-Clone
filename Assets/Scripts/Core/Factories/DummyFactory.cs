using System.Collections.Generic;
using Core.Actors;
using Core.Factories.Interface;
using Core.Factories.Pools;
using UnityEngine;

namespace Core.Factories
{
    public class DummyFactory : ObjectFactory<Dummy>, IDummyFactory
    {
        private List<Dummy> _allDummies = new();
        public override void PreInitialize()
        {
            Pool = new ObjectPool<Dummy>(ObjPrefab, ParentTr, 16);
            _allDummies = new List<Dummy>(16);
        }
        
        public void PopulateDummies(ColorType[,] colorTypes, List<Dummy> dummies)
        {
            // Process the grid with column-to-row traversal
            for (int i = 0; i < colorTypes.GetLength(1); i++) // Columns
            {
                for (int j = 0; j < colorTypes.GetLength(0); j++) // Rows
                {
                    Vector2Int coordinate = new Vector2Int(i, j);
                    var gridType = colorTypes[j, i];

                    var gridObject = GenerateDummy(gridType, coordinate);
                    if (gridObject != null)
                    {
                        dummies.Add(gridObject);
                    }
                }
            }
        }
        
        private Dummy GenerateDummy(ColorType colorType, Vector2Int dummyCoordinate)
        {
            var dummy = CreateObj();
            dummy.SetAttributes(dummyCoordinate, colorType);
            return dummy;
        }

        public override Dummy CreateObj()
        {
            var item = base.CreateObj();
            _allDummies.Add(item);
            return item;
        }

        public override void DestroyObj(Dummy emptyItem)
        {
            base.DestroyObj(emptyItem);
            emptyItem.SetAttributes(-Vector2Int.one, ColorType.Empty);
            _allDummies.Remove(emptyItem);
        }

        public void DestroyAllItems()
        {
            var itemsToDestroy = new List<Dummy>(_allDummies);
            base.DestroyObjs(itemsToDestroy);
            _allDummies.Clear();
        }
    }
}
