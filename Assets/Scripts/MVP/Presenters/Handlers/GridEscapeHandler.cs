
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
        private bool[,] _visited;
        
        private static readonly Dictionary<Direction, Vector2Int> _directionOffsets = new Dictionary<Direction, Vector2Int>
        {
            { Direction.Up,    new Vector2Int(0, 1) },  // Move up a row
            { Direction.Down,  new Vector2Int(0, -1) }, // Move down a row
            { Direction.Left,  new Vector2Int(-1, 0) }, // Move left a column
            { Direction.Right, new Vector2Int(1, 0) }   // Move right a column
        };

        public void Initialize(Dummy[,] grid)
        {
            _grid = grid;
            _columnCount = _grid.GetLength(0);
            _rowCount = _grid.GetLength(1);
            _visited = new bool[_columnCount, _rowCount];
        }
        
        public Dictionary<Dummy, List<Vector2Int>> FindEscapePath()
        {
            Vector2Int start = Vector2Int.one;
            var pathDict = new Dictionary<Dummy, List<Vector2Int>>();
            var visited = new bool[_columnCount, _rowCount];

            // Ensure we can only start from a NotEmpty cell
            if (_grid[start.x, start.y].ColorType == ColorType.Empty)
                return pathDict; // Can't start from Empty

            // Start DFS from the selected cell
            DFS(start, pathDict, visited);

            return pathDict;
        }

        private bool DFS(Vector2Int current, Dictionary<Dummy, List<Vector2Int>> pathDict, bool[,] visited)
        {
            int col = current.x;
            int row = current.y;
            var currentDummy = _grid[col, row];

            // Base conditions: Out of bounds, already visited, or blocked by non-empty cell
            if (row < 0 || row >= _rowCount || col < 0 || col >= _columnCount || visited[col, row] || _grid[col, row].ColorType != ColorType.Empty)
            {
                return false;
            }

            // Mark current cell as visited
            visited[col, row] = true;

            // If we are in the 0th row and the cell is Empty, we found a valid path
            if (row == 0 && _grid[col, row].ColorType == ColorType.Empty)
            {
                pathDict[currentDummy] = new List<Vector2Int> { current };
                return true;
            }

            // Record the path from current cell (start building path from here)
            pathDict[currentDummy] = new List<Vector2Int> { current };

            // Explore all 4 directions (up, down, left, right)
            foreach (var direction in _directionOffsets.Values)
            {
                Vector2Int newPosition = new Vector2Int(col, row) + direction;

                // If DFS in one direction succeeds, add that to the path
                if (DFS(newPosition, pathDict, visited))
                {
                    // Add the path from the next cell to the current path
                    pathDict[currentDummy].AddRange(pathDict[_grid[newPosition.x, newPosition.y]]);
                    return true;
                }
            }

            // Backtrack if no valid path is found
            visited[col, row] = false;
            return false;
        }



    }
}
