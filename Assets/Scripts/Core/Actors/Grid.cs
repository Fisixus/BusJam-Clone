using UnityEngine;

namespace Core.Actors
{
    public class Grid : MonoBehaviour
    {
        [field: SerializeField] public Vector2Int Coordinate { get; set; }
        
        public void SetWorldPosition()
        {
            
        }
        
        public void SetAttributes(Vector2Int newCoord)
        {
            Coordinate = newCoord;
            name = ToString();
        }
        
        public override string ToString()
        {
            return $"Column{Coordinate.x},Row{Coordinate.y}";
        }
    }
}
