using System.Collections.Generic;
using Core.Factories.Interface;
using Core.Factories.Pools;
using UnityEngine;
using Grid = Core.Actors.Grid;

namespace Core.Factories
{
    public class GridFactory : ObjectFactory<Grid>, IGridFactory
    {
        [field: SerializeField] public Vector2 Spacing { get; private set; } = new Vector2(0.8f, 1.25f);

        private List<Grid> _allGrids = new();

        public override void PreInitialize()
        {
            Pool = new ObjectPool<Grid>(ObjPrefab, ParentTr, 16);
        }

        public void PopulateGrids(ColorType[,] colorTypes, List<Grid> grids)
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
                    var gridObject = GenerateGrid(coordinate);
                    gridObject.SetStartPosition(startX, Spacing);

                    if (gridObject != null)
                    {
                        grids.Add(gridObject);
                    }
                }
            }
        }

        private Grid GenerateGrid(Vector2Int gridCoordinate)
        {
            var grid = CreateObj();
            grid.SetAttributes(gridCoordinate);
            return grid;
        }

        public override Grid CreateObj()
        {
            var item = base.CreateObj();
            _allGrids.Add(item);
            return item;
        }

        public override void DestroyObj(Grid emptyItem)
        {
            base.DestroyObj(emptyItem);
            emptyItem.SetAttributes(-Vector2Int.one);
            _allGrids.Remove(emptyItem);
        }

        public void DestroyAllGrids()
        {
            var itemsToDestroy = new List<Grid>(_allGrids);
            base.DestroyObjs(itemsToDestroy);
            _allGrids.Clear();
        }
    }
}