using System.Collections.Generic;
using UnityEngine;
using Grid = Core.Actors.Grid;

namespace Core.Factories.Interface
{
    public interface IGridFactory : IFactory<Grid>
    {
        public Vector2 Spacing { get; }
        public void PopulateGrids(ColorType[,] colorTypes, List<Grid> grids);
        public void DestroyAllGrids();
    }
}
