using System.Collections.Generic;
using Core.Factories.Interface;
using Core.Factories.Pools;
using UnityEngine;
using Grid = Core.Actors.Grid;

namespace Core.Factories
{
    public class GridFactory : ObjectFactory<Grid>, IGridFactory
    {
        public override void PreInitialize()
        {
            Pool = new ObjectPool<Grid>(ObjPrefab, ParentTr, 16);
        }
        
        public void PopulateGrids(ColorType[,] colorTypes, List<Grid> grids)
        {
            // Process the grid with column-to-row traversal
            for (int i = 0; i < colorTypes.GetLength(1); i++) // Columns
            {
                for (int j = 0; j < colorTypes.GetLength(0); j++) // Rows
                {
                    Vector2Int coordinate = new Vector2Int(i, j);
                    var gridObject = GenerateGrid(coordinate);
                    if (gridObject != null)
                    {
                        grids.Add(gridObject);
                    }
                }
            }
        }
        
        private Grid GenerateGrid(Vector2Int dummyCoordinate)
        {
            var dummy = CreateObj();
            dummy.SetAttributes(dummyCoordinate);
            return dummy;
        }

    }
}