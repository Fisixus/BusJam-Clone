using System;
using UnityEngine;

namespace Core.Actors
{
    public class Dummy : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public Renderer JointRenderer { get; private set; }
        [field: SerializeField] public Renderer SurfaceRenderer { get; private set; }
        [field: SerializeField] public Vector2Int Coordinate { get; set; }
        [field: SerializeField] public ColorType ColorType { get; set; }
        
        public void SetWorldPosition(float startX, Vector2 spacing)
        {
            transform.localPosition = new Vector3(startX + Coordinate.x * spacing.x, transform.localPosition.y,
                Coordinate.y * -spacing.y);
        }
        
        public void SetAttributes(Vector2Int newCoord, ColorType colorType)
        {
            Coordinate = newCoord;
            ColorType = colorType;
            name = ToString();
        }

        public void SetColor(ColorDataSO colorData)
        {
            JointRenderer.material.color = colorData.Colors[ColorType];
            SurfaceRenderer.material.color = colorData.Colors[ColorType];
        }
        
        public override string ToString()
        {
            return $"Dummy:{ColorType}, Column{Coordinate.x},Row{Coordinate.y}";
        }
    }
}
