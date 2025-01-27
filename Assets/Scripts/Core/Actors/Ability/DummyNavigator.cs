using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Core.Actors.Ability
{
    public class DummyNavigator : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public float Speed { get; private set; } = 2f;  // Speed in units per second
        private static readonly int SpeedAnim = Animator.StringToHash("Speed");
        
        private Quaternion _originalRotation; // Store original rotation

        private void Start()
        {
            _originalRotation = transform.rotation; // Save initial rotation
        }
        public void MoveAlongPath(List<Vector3> path)
        {
            if (path == null || path.Count < 2) return;
            Animator.SetFloat(SpeedAnim, Speed);
            float totalDistance = CalculateTotalDistance(path);
            float duration = totalDistance / Speed; // Time = Distance / Speed
            transform.DOKill();
            transform.DOPath(path.ToArray(), duration)
                .SetLookAt(0.01f) // Makes the character rotate along the path
                .SetEase(Ease.Linear).OnComplete(() =>
                {
                    Animator.SetFloat(SpeedAnim, 0);
                    ResetRotation();
                });
        }

        private float CalculateTotalDistance(List<Vector3> path)
        {
            float distance = 0f;
            for (int i = 0; i < path.Count - 1; i++)
            {
                distance += Vector3.Distance(path[i], path[i + 1]);
            }
            return distance;
        }

        private void ResetRotation()
        {
            transform.DORotateQuaternion(_originalRotation, 0.2f)
                .SetEase(Ease.OutQuad); // Smoothly return to original rotation
        }
    }
}
