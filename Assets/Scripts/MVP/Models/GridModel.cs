using System.Collections.Generic;
using Core;
using Core.Actors;
using MVP.Models.Interface;
using UnityEngine;

namespace MVP.Models
{
    public class GridModel: IGridModel
    {
        public Dummy[,] Grid { get; private set; } // x:column, y:row
        private int _columnCount;
        private int _rowCount;
        
        public void Initialize(List<Dummy> dummies, int columns, int rows)
        {
            _columnCount = columns;
            _rowCount = rows;
            Grid = new Dummy[_columnCount, _rowCount];
            for (int i = 0; i < _columnCount; i++) // Columns
            {
                for (int j = 0; j < _rowCount; j++) // Rows
                {
                    Grid[i, j] = dummies[i * _rowCount + j];
                }
            }

            
        }
    }
}
