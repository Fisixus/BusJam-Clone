using System.Collections.Generic;
using Core.Actors;
using MVP.Presenters.Handlers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVP.Presenters
{
    public class GridPresenter
    {
        private readonly GridEscapeHandler _gridEscapeHandler;

        public GridPresenter(GridEscapeHandler gridEscapeHandler)
        {
            _gridEscapeHandler = gridEscapeHandler;
            
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
            var escapePaths = GetAllRunnableDummies();
    
            // Check if the touched dummy can escape
            if (escapePaths.TryGetValue(touchedDummy, out var path) && path != null)
            {
                // TODO: Update grid position to empty
                // TODO: Move dummy to escape position
                // TODO: After dummy reaches escape position, move him either through bus or waiting line
            }
            else
            {
                // TODO: Show angry person emoji
            }

            // Highlight runnable dummies after touch interaction
            HighlightRunnableDummies(escapePaths);
        }

        public Dictionary<Dummy, List<Vector2Int>> GetAllRunnableDummies()
        {
            return _gridEscapeHandler.GetAllEscapePaths();
        }

        public void HighlightRunnableDummies(Dictionary<Dummy, List<Vector2Int>> runnableDummies)
        {
            foreach (var kv in runnableDummies)
            {
                kv.Key.SetOutline(true);
            }
        }


    }
}
