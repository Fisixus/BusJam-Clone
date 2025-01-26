using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Core.Actors.Data
{
    [CreateAssetMenu(fileName = "BusData_00", menuName = "Data/New BusData")]
    public class BusDataSO : ScriptableObject
    {
        [field: SerializeField] public SerializedDictionary<int, float> OrderXPositions { get; private set; }
    }
}