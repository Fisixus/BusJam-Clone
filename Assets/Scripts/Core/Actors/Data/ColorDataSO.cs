using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Core.Actors.Data
{
    [CreateAssetMenu(fileName = "ColorData_00", menuName = "Data/New ColorData")]
    public class ColorDataSO : ScriptableObject
    {
        [field: SerializeField] public SerializedDictionary<ColorType, Color> Colors { get; private set; }
    }
}