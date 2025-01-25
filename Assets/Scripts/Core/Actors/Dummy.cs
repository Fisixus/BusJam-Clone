using UnityEngine;

namespace Core.Actors
{
    public class Dummy : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public Vector2Int Coordinate { get; set; }
        [field: SerializeField] public ColorType ColorType { get; set; }
        
        public void SetWorldPosition()
        {
            
        }
        
        public void SetAttributes(Vector2Int newCoord, ColorType colorType)
        {
            Coordinate = newCoord;
            ColorType = colorType;
            name = ToString();
        }
        public override string ToString()
        {
            return $"Column{Coordinate.x},Row{Coordinate.y}, Color:{ColorType}";
        }
    }
}
