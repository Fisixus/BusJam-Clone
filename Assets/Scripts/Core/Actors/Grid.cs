using UnityEngine;

namespace Core.Actors
{
    public class Grid : MonoBehaviour
    {
        [field: SerializeField] public Vector2Int Coordinate { get; set; }
        
        public void SetWorldPosition(float startX, Vector2 spacing)
        {
            transform.localPosition = new Vector3(startX + Coordinate.x * spacing.x, transform.localPosition.y,
                Coordinate.y * -spacing.y);
        }
        
        public void SetAttributes(Vector2Int newCoord)
        {
            Coordinate = newCoord;
            name = ToString();
        }
        
        public override string ToString()
        {
            return $"Grid, Column{Coordinate.x},Row{Coordinate.y}";
        }
    }
}
