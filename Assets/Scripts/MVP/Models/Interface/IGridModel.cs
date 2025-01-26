using System.Collections.Generic;
using Core;
using Core.Actors;

namespace MVP.Models.Interface
{
    public interface IGridModel
    {
        public Dummy[,] Grid { get; } // x:column, y:row
        public void Initialize(List<Dummy> dummies, int columns, int rows);
    }
}
