using System.Collections.Generic;
using Core.Actors;
using Core.Helpers;
using UnityEngine;

namespace MVP.Presenters.Handlers
{
    public class GridEscapeHandler
    {
        private Dummy[,] _grid;
        private int _rowCount;
        private int _columnCount;

        private static readonly Dictionary<Direction, Vector2Int> _directionOffsets =
            new Dictionary<Direction, Vector2Int>
            {
                { Direction.Up, new Vector2Int(0, 1) }, // Move up a row
                { Direction.Down, new Vector2Int(0, -1) }, // Move down a row
                { Direction.Left, new Vector2Int(-1, 0) }, // Move left a column
                { Direction.Right, new Vector2Int(1, 0) } // Move right a column
            };

        public void Initialize(Dummy[,] grid)
        {
            _grid = grid;
            _columnCount = _grid.GetLength(0);
            _rowCount = _grid.GetLength(1);
        }


        public Dictionary<Dummy, List<Vector2Int>> GetAllEscapePaths()
        {
            Dictionary<Dummy, List<Vector2Int>> escapePaths = new Dictionary<Dummy, List<Vector2Int>>();
            Dictionary<Vector2Int, List<Vector2Int>>
                memoizedPaths = new Dictionary<Vector2Int, List<Vector2Int>>(); // Memoization

            for (int col = 0; col < _columnCount; col++)
            {
                for (int row = 0; row < _rowCount; row++)
                {
                    Vector2Int pos = new Vector2Int(col, row);
                    if (_grid[col, row].IsLeftGrid) continue;

                    if (memoizedPaths.TryGetValue(pos, out List<Vector2Int> cachedPath))
                    {
                        escapePaths[_grid[col, row]] = new List<Vector2Int>(cachedPath);
                        continue;
                    }

                    List<Vector2Int> path = new List<Vector2Int>();
                    if (TryFindEscapePath(pos, path, memoizedPaths))
                    {
                        escapePaths[_grid[col, row]] = new List<Vector2Int>(path);
                        memoizedPaths[pos] = path; // Store the shortest path in the memoization table
                    }
                }
            }

            return escapePaths;
        }

        /// <summary>
        /// Attempts to find the shortest escape path using BFS with memoization.
        /// </summary>
        private bool TryFindEscapePath(Vector2Int start, List<Vector2Int> path,
            Dictionary<Vector2Int, List<Vector2Int>> memoizedPaths)
        {
            if (start.y == 0) // Dummies in the first row escape immediately
            {
                path.Add(start);
                return true;
            }

            if (memoizedPaths.TryGetValue(start, out List<Vector2Int> cachedPath))
            {
                path.AddRange(cachedPath);
                return true;
            }

            Queue<(Vector2Int, List<Vector2Int>)> queue = new Queue<(Vector2Int, List<Vector2Int>)>();
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
            queue.Enqueue((start, new List<Vector2Int> { start }));
            visited.Add(start);
            var targetDummy = _grid[start.x, start.y];

            while (queue.Count > 0)
            {
                var (current, currentPath) = queue.Dequeue();

                foreach (Vector2Int next in GetValidNeighbors(current, targetDummy))
                {
                    if (visited.Contains(next)) continue;

                    List<Vector2Int> newPath = new List<Vector2Int>(currentPath) { next };

                    if (IsExitPoint(next))
                    {
                        path.Clear();
                        path.AddRange(newPath);
                        memoizedPaths[start] = new List<Vector2Int>(newPath); // Store the computed path
                        return true;
                    }

                    queue.Enqueue((next, newPath));
                    visited.Add(next);
                }
            }

            return false;
        }

        /// <summary>
        /// Retrieves valid neighboring cells that the dummy can move to.
        /// </summary>
        private IEnumerable<Vector2Int> GetValidNeighbors(Vector2Int current, Dummy targetDummy)
        {
            foreach (var direction in _directionOffsets)
            {
                Vector2Int next = current + direction.Value;
                if (!IsWithinBounds(next.x, next.y)) continue;

                if (_grid[next.x, next.y].IsLeftGrid || _grid[next.x, next.y].Equals(targetDummy))
                {
                    yield return next;
                }
            }
        }

        /// <summary>
        /// Checks if the position is a valid exit (row 0 and empty).
        /// </summary>
        private bool IsExitPoint(Vector2Int pos)
        {
            return pos.y == 0 && _grid[pos.x, pos.y].IsLeftGrid;
        }

        /// <summary>
        /// Checks if a given position is within the grid boundaries.
        /// </summary>
        private bool IsWithinBounds(int col, int row)
        {
            return col >= 0 && col < _columnCount && row >= 0 && row < _rowCount;
        }
    }
}