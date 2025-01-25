using System.Collections.Generic;
using Core.Actors;
using Core.Factories;
using Core.Factories.Interface;
using Core.LevelSerialization;

namespace MVP.Presenters.Handlers
{
    public class LevelSetupHandler
    {
        private readonly IDummyFactory _dummyFactory;
        private readonly IGridFactory _gridFactory;
        private readonly IBusFactory _busFactory;

        public LevelSetupHandler(IDummyFactory dummyFactory, IGridFactory gridFactory, IBusFactory busFactory)
        {
            _dummyFactory = dummyFactory;
            _gridFactory = gridFactory;
            _busFactory = busFactory;
        }
        public void Initialize(LevelInfo levelInfo)
        {
            // Process the grid in a single loop
            var dummyColors = levelInfo.Dummies;

            List<Dummy> dummies = new List<Dummy>(16);
            List<Grid> grids = new List<Grid>(16);
            
            //_dummyFactory.DestroyAllDummies();
            _gridFactory.DestroyAllGrids();
            
            //_dummyFactory.PopulateDummies(dummyColors, dummies);
            _gridFactory.PopulateGrids(dummyColors, grids);
        }
    }
}