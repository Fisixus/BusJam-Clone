using UnityEngine;

namespace Core.Actors
{
    public class BusWaitingSpot : MonoBehaviour
    {
        [field: SerializeField] public int Order { get; set; }
        [field: SerializeField] public Dummy Dummy { get; set; }
        [field: SerializeField] public bool IsAvailable { get; set; }

        public void SetStartPosition(float startX, Vector2 spacing)
        {
            transform.localPosition = new Vector3(startX + Order * spacing.x, transform.localPosition.y,
                transform.localPosition.z);
        }
    }
}