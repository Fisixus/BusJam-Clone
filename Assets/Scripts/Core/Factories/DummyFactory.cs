using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Core.Actors;
using Core.Actors.Data;
using Core.Factories.Interface;
using Core.Factories.Pools;
using UnityEngine;

namespace Core.Factories
{
    public class DummyFactory : ObjectFactory<Dummy>, IDummyFactory
    {
        [field: SerializeField]
        public ColorDataSO ColorData { get; private set; }

        [field:SerializeField]
        public Vector2 Spacing { get; private set; } = new Vector2(0.8f, 1.25f);
        
        private List<Dummy> _allDummies = new();
        public override void PreInitialize()
        {
            Pool = new ObjectPool<Dummy>(ObjPrefab, ParentTr, 16);
            _allDummies = new List<Dummy>(16);
        }
        
        public void PopulateDummies(ColorType[,] colorTypes, List<Dummy> dummies)
        {
            var columns = colorTypes.GetLength(1);
            var rows = colorTypes.GetLength(0);
            var startX = -((columns - 1) * Spacing.x) / 2;

            // Process the grid with column-to-row traversal
            for (int i = 0; i < columns; i++) // Columns
            {
                for (int j = 0; j < rows; j++) // Rows
                {
                    Vector2Int coordinate = new Vector2Int(i, j);
                    var colorType = colorTypes[j, i];
                    var dummy = GenerateDummy(colorType, coordinate);
                    dummy.SetStartPosition(startX, Spacing);
                    if (dummy != null)
                    {
                        dummies.Add(dummy);
                    }
                }
            }
        }
        
        private Dummy GenerateDummy(ColorType colorType, Vector2Int dummyCoordinate)
        {
            var dummy = CreateObj();
            dummy.IsLeftGrid = false;
            dummy.SetTouchAbility(true);
            dummy.SetAttributes(dummyCoordinate, colorType);
            dummy.SetColor(ColorData);
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
            emptyItem.SetAttributes(-Vector2Int.one, ColorType.None);
            _allDummies.Remove(emptyItem);
        }

        public void DestroyAllDummies()
        {
            var itemsToDestroy = new List<Dummy>(_allDummies);
            base.DestroyObjs(itemsToDestroy);
            _allDummies.Clear();
        }
    }
}
