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
        private readonly IBusWaitingSpotFactory _waitingSpotFactory;
        private readonly IGridModel _gridModel;
        private readonly IBusModel _busModel;
        private readonly GridEscapeHandler _gridEscapeHandler;
        private readonly GridPresenter _gridPresenter;

        public LevelSetupHandler(IDummyFactory dummyFactory, IGridFactory gridFactory, IBusFactory busFactory, IBusWaitingSpotFactory waitingSpotFactory,
            IGridModel gridModel, IBusModel busModel, GridEscapeHandler gridEscapeHandler, GridPresenter gridPresenter)
        {
            _dummyFactory = dummyFactory;
            _gridFactory = gridFactory;
            _busFactory = busFactory;
            _waitingSpotFactory = waitingSpotFactory;
            _gridModel = gridModel;
            _busModel = busModel;
            _gridEscapeHandler = gridEscapeHandler;
            _gridPresenter = gridPresenter;
        }
        public void Initialize(LevelInfo levelInfo)
        {
            // Process the grid in a single loop

            var busColors = levelInfo.BusOrder;
            var dummyColors = levelInfo.Dummies;
            var rows = dummyColors.GetLength(0);
            var cols = dummyColors.GetLength(1);

            List<Dummy> dummies = new List<Dummy>(16);
            List<Grid> grids = new List<Grid>(16);
            List<BusWaitingSpot> spots = new List<BusWaitingSpot>(5);
            List<Bus> buses = new List<Bus>(4);
            
            _dummyFactory.DestroyAllDummies();
            _gridFactory.DestroyAllGrids();
            _busFactory.DestroyAllBuses();
            
            _dummyFactory.PopulateDummies(dummyColors, dummies);
            _gridFactory.PopulateGrids(dummyColors, grids);
            _busFactory.PopulateBuses(busColors, buses);
            _waitingSpotFactory.PopulateSpots(spots);
            
            _gridModel.InitializeDummies(dummies, cols, rows);
            _gridModel.InitializeGrids(grids, cols, rows);
            _gridModel.InitializeBusWaitingSpots(spots);
            _busModel.Initialize(buses);
            _gridEscapeHandler.Initialize(_gridModel.Dummies);
            
            _gridPresenter.SetAllRunnableDummies();
            _gridPresenter.HighlightRunnableDummies();

        }
    }
}