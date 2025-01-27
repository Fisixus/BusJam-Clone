using System.Collections.Generic;
using Core;
using Core.Actors;

namespace MVP.Models.Interface
{
    public interface IGridModel
    {
        public Queue<BusWaitingSpot> BusWaitingSpots { get; }
        public Grid[,] Grid { get; } // x:column, y:row
        public Dummy[,] Dummies { get; } // x:column, y:row
        public void InitializeDummies(List<Dummy> dummies, int columns, int rows);
        public void InitializeGrids(List<Grid> grids, int columns, int rows);
        public void InitializeBusWaitingSpots(List<BusWaitingSpot> spots);
    }
}
