using System.Collections.Generic;
using Core.Actors;
using MVP.Models.Interface;

namespace MVP.Models
{
    public class StationModel : IStationModel
    {
        public Dummy[,] Dummies { get; private set; } // x:column, y:row
        public Grid[,] Grid { get; private set; } // x:column, y:row
        public BusWaitingSpot[] BusWaitingSpots { get; private set; }

        private int _columnCount;
        private int _rowCount;


        public void InitializeDummies(List<Dummy> dummies, int columns, int rows)
        {
            _columnCount = columns;
            _rowCount = rows;
            Dummies = new Dummy[_columnCount, _rowCount];
            for (int i = 0; i < _columnCount; i++) // Columns
            {
                for (int j = 0; j < _rowCount; j++) // Rows
                {
                    Dummies[i, j] = dummies[i * _rowCount + j];
                }
            }
        }

        public void InitializeGrids(List<Grid> grids, int columns, int rows)
        {
            _columnCount = columns;
            _rowCount = rows;
            Grid = new Grid[_columnCount, _rowCount];
            for (int i = 0; i < _columnCount; i++) // Columns
            {
                for (int j = 0; j < _rowCount; j++) // Rows
                {
                    Grid[i, j] = grids[i * _rowCount + j];
                }
            }
        }

        public void InitializeBusWaitingSpots(List<BusWaitingSpot> spots)
        {
            BusWaitingSpots = new BusWaitingSpot[spots.Count];
            for (int i = 0; i < spots.Count; i++)
            {
                BusWaitingSpots[i] = spots[i];
            }
        }
    }
}