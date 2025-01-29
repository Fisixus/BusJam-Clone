using Core.Actors.Data;
using UnityEngine;

namespace Core.Actors
{
    public class BusChair : MonoBehaviour
    {
        [field: SerializeField] public bool IsAvailable { get; set; }
        [field: SerializeField] public ParticleSystem PlacingPS { get; set; }
        [field: SerializeField] public Dummy ChairOwner { get; set; }

        public void ResetChair()
        {
            ChairOwner.gameObject.SetActive(false);
            IsAvailable = true;
        }

        public void SetChairOwner(ColorType colorType, ColorDataSO colorData)
        {
            ChairOwner.gameObject.SetActive(true);
            IsAvailable = false;
            ChairOwner.ColorType = colorType;
            ChairOwner.SetColor(colorData);
            PlacingPS.Play();
            ChairOwner.Navigator.SetAnimationState(DummyAnimations.Sitting);
        }
    }
}