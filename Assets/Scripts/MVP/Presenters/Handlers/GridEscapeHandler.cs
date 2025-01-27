
using System.Collections.Generic;
using Core;
using Core.Actors;
using UnityEngine;

namespace MVP.Presenters.Handlers
{
    public class GridEscapeHandler
    {
        private Dummy[,] _grid;
        private int _rowCount;
        private int _columnCount;
        
        private static readonly Dictionary<Direction, Vector2Int> _directionOffsets = new Dictionary<Direction, Vector2Int>
        {
            { Direction.Up,    new Vector2Int(0, 1) },  // Move up a row
            { Direction.Down,  new Vector2Int(0, -1) }, // Move down a row
            { Direction.Left,  new Vector2Int(-1, 0) }, // Move left a column
            { Direction.Right, new Vector2Int(1, 0) }   // Move right a column
        };

        private Dictionary<Dummy, List<Vector2Int>> _escapePaths = new();
        
        public void Initialize(Dummy[,] grid)
        {
            _grid = grid;
            _columnCount = _grid.GetLength(0);
            _rowCount = _grid.GetLength(1);
        }


        public Dictionary<Dummy, List<Vector2Int>> GetAllEscapePaths()
        {
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
            Dictionary<Dummy, List<Vector2Int>> escapePaths = new Dictionary<Dummy, List<Vector2Int>>();

            for (int col = 0; col < _columnCount; col++)  // Iterate by column (x)
            {
                for (int row = 0; row < _rowCount; row++)  // Iterate by row (y)
                {
                    Vector2Int pos = new Vector2Int(col, row);
                    if (!visited.Contains(pos) && _grid[col, row].ColorType != ColorType.Empty)
                    {
                        List<Vector2Int> path = new List<Vector2Int>();
                        bool canEscape = ExplorePath(pos, visited, path);

                        if (canEscape)
                        {
                            escapePaths[_grid[col, row]] = new List<Vector2Int>(path);
                        }
                    }
                }
            }

            return escapePaths;
        }

        /// <summary>
        /// Explores the grid using BFS to determine if a dummy can escape.
        /// If it's already in the first row, it escapes immediately without adding empty cells.
        /// </summary>
        private bool ExplorePath(Vector2Int start, HashSet<Vector2Int> visited, List<Vector2Int> path)
        {
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            queue.Enqueue(start);
            var targetDummy = _grid[start.x, start.y];  // Get the dummy in this cell
            bool canEscape = false;

            // If the dummy is already in the first row, escape immediately
            if (start.y == 0)
            {
                path.Add(start);
                return true;
            }

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                if (visited.Contains(current)) continue;

                visited.Add(current);
                path.Add(current);

                foreach (var direction in _directionOffsets)
                {
                    Vector2Int next = current + direction.Value;

                    if (!IsWithinBounds(next.x, next.y) || visited.Contains(next))
                        continue;

                    // If the next cell is empty, check if it's an escape point
                    if (_grid[next.x, next.y].ColorType == ColorType.Empty)
                    {
                        if (next.y == 0) // If this is an empty exit, finalize path
                        {
                            path.Add(next); // Only add the empty exit if needed
                            return true;
                        }
                        else
                        {
                            queue.Enqueue(next);
                            visited.Add(next);
                        }
                    }
                    // If the next cell contains the same dummy type, continue search
                    else if (_grid[next.x, next.y].Equals(targetDummy))
                    {
                        queue.Enqueue(next);
                    }
                }
            }

            return canEscape;
        }


        /// <summary>
        /// Checks if a given position is within grid boundaries.
        /// </summary>
        private bool IsWithinBounds(int col, int row)
        {
            return col >= 0 && col < _columnCount && row >= 0 && row < _rowCount;
        }

    }
}
