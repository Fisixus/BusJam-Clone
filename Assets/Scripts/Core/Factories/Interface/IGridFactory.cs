using System.Collections.Generic;
using Grid = Core.Actors.Grid;

namespace Core.Factories.Interface
{
    public interface IGridFactory : IFactory<Grid>
    {
        public void PopulateGrids(ColorType[,] colorTypes, List<Grid> grids);
    }
}
