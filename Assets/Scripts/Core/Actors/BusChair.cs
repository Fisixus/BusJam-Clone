using Core.Actors.Data;
using UnityEngine;

namespace Core.Actors
{
    public class BusChair : MonoBehaviour
    {
        [field: SerializeField] public bool IsAvailable { get;  set; }
        [field: SerializeField] public ParticleSystem PlacingPS { get; set; }
        [field: SerializeField] public Dummy ChairOwner { get; set; }

        public void ResetChair()
        {
            ChairOwner.gameObject.SetActive(false);
            IsAvailable = true;
        }
        
        public void SetChairOwner(Dummy dummy, ColorDataSO colorData)
        {
            ChairOwner.ColorType = dummy.ColorType;
            ChairOwner.SetColor(colorData);//TODO:
            PlacingPS.Play();
            ChairOwner.Navigator.SetAnimationState(DummyAnimations.Sitting);
            ChairOwner.gameObject.SetActive(true);
            IsAvailable = false;
        }
    }
}
