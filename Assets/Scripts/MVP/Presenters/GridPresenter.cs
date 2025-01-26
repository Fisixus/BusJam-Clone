using System.Collections.Generic;
using Core;
using Core.Actors;
using MVP.Models.Interface;
using MVP.Presenters.Handlers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVP.Presenters
{
    public class GridPresenter
    {
        private readonly GridEscapeHandler _gridEscapeHandler;
        private readonly IGridModel _gridModel;

        private Dictionary<Dummy, List<Vector2Int>> _runnableDummies = new();

        public GridPresenter(GridEscapeHandler gridEscapeHandler, IGridModel gridModel)
        {
            _gridEscapeHandler = gridEscapeHandler;
            _gridModel = gridModel;
            
            UserInput.OnDummyTouched += OnTouch;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            Dispose();
        }

        private void Dispose()
        {
            // Unsubscribe from static and instance events
            UserInput.OnDummyTouched -= OnTouch;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
        
        private void OnTouch(Dummy touchedDummy)
        {
            Debug.Log("Touched:" + touchedDummy);

            // Check if the touched dummy can escape
            if (_runnableDummies.TryGetValue(touchedDummy, out var path) && path != null)
            {
                // Update grid position to empty
                var coord = touchedDummy.Coordinate;
                _gridModel.Grid[coord.x, coord.y].ColorType = ColorType.Empty;
                // TODO: Move dummy to escape position
                // TODO: After dummy reaches escape position, move him either through bus or waiting line
            }
            else
            {
                // Show angry person emoji
                touchedDummy.PlayEmojiAnimation();
            }

            // Highlight runnable dummies after touch interaction
            SetAllRunnableDummies();
            HighlightRunnableDummies();
        }

        public void SetAllRunnableDummies()
        {
            _runnableDummies = _gridEscapeHandler.GetAllEscapePaths();
        }

        public void HighlightRunnableDummies()
        {
            foreach (var kv in _runnableDummies)
            {
                Debug.Log(kv.Key);
                kv.Key.SetOutline(true);
            }
        }


    }
}
