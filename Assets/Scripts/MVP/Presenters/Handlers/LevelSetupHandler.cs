using System.Collections.Generic;
using Core.Actors;
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
        private readonly IStationModel _stationModel;
        private readonly IBusModel _busModel;
        private readonly GridEscapeHandler _gridEscapeHandler;
        private readonly StationPresenter _stationPresenter;

        public LevelSetupHandler(IDummyFactory dummyFactory, IGridFactory gridFactory, IBusFactory busFactory,
            IBusWaitingSpotFactory waitingSpotFactory,
            IStationModel stationModel, IBusModel busModel, GridEscapeHandler gridEscapeHandler,
            StationPresenter stationPresenter)
        {
            _dummyFactory = dummyFactory;
            _gridFactory = gridFactory;
            _busFactory = busFactory;
            _waitingSpotFactory = waitingSpotFactory;
            _stationModel = stationModel;
            _busModel = busModel;
            _gridEscapeHandler = gridEscapeHandler;
            _stationPresenter = stationPresenter;
        }

        public void Initialize(LevelInfo levelInfo)
        {
            // Process the grid in a single loop

            var busColors = levelInfo.Buses;
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
            _waitingSpotFactory.DestroyAllWaitingSpots();

            _dummyFactory.PopulateDummies(dummyColors, dummies);
            _gridFactory.PopulateGrids(dummyColors, grids);
            _busFactory.PopulateBuses(busColors, buses);
            _waitingSpotFactory.PopulateSpots(spots);

            _stationModel.InitializeDummies(dummies, cols, rows);
            _stationModel.InitializeGrids(grids, cols, rows);
            _stationModel.InitializeBusWaitingSpots(spots);
            _busModel.Initialize(buses);
            _gridEscapeHandler.Initialize(_stationModel.Dummies);

            _stationPresenter.SetAllRunnableDummies();
            _stationPresenter.HighlightRunnableDummies();
        }
    }
}