using System.Collections.Generic;
using Core.Actors;
using Core.Factories;
using Core.Factories.Interface;
using Core.LevelSerialization;
using MVP.Models.Interface;

namespace MVP.Presenters.Handlers
{
    public class LevelSetupHandler
    {
        private readonly IDummyFactory _dummyFactory;
        private readonly IGridFactory _gridFactory;
        private readonly IBusFactory _busFactory;
        private readonly IGridModel _gridModel;
        private readonly GridEscapeHandler _gridEscapeHandler;
        private readonly GridPresenter _gridPresenter;

        public LevelSetupHandler(IDummyFactory dummyFactory, IGridFactory gridFactory, IBusFactory busFactory,
            IGridModel gridModel, GridEscapeHandler gridEscapeHandler, GridPresenter gridPresenter)
        {
            _dummyFactory = dummyFactory;
            _gridFactory = gridFactory;
            _busFactory = busFactory;
            _gridModel = gridModel;
            _gridEscapeHandler = gridEscapeHandler;
            _gridPresenter = gridPresenter;
        }
        public void Initialize(LevelInfo levelInfo)
        {
            // Process the grid in a single loop
            var dummyColors = levelInfo.Dummies;
            var rows = dummyColors.GetLength(0);
            var cols = dummyColors.GetLength(1);

            List<Dummy> dummies = new List<Dummy>(16);
            List<Grid> grids = new List<Grid>(16);
            
            _dummyFactory.DestroyAllDummies();
            _gridFactory.DestroyAllGrids();
            
            _dummyFactory.PopulateDummies(dummyColors, dummies);
            _gridFactory.PopulateGrids(dummyColors, grids);
            
            _gridModel.Initialize(dummies, cols, rows);
            _gridEscapeHandler.Initialize(_gridModel.Grid);
            
            _gridPresenter.SetAllRunnableDummies();
            _gridPresenter.HighlightRunnableDummies();
        }
    }
}