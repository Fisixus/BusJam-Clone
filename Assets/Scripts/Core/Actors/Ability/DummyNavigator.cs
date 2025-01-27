using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Core.Actors.Ability
{
    public class DummyNavigator : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public float Speed { get; private set; } = 2f;  // Speed in units per second
        
        private Quaternion _originalRotation; // Store original rotation

        private void Start()
        {
            _originalRotation = transform.rotation; // Save initial rotation
        }

        public void SetAnimationState(DummyAnimations animState)
        {
            switch (animState)
            {
                case DummyAnimations.Idle:
                    Animator.SetTrigger(DummyAnimations.Idle.ToString());
                    break;
                case DummyAnimations.Sitting:
                    Animator.SetTrigger(DummyAnimations.Sitting.ToString());
                    break;
                case DummyAnimations.Running:
                    Animator.SetTrigger(DummyAnimations.Running.ToString());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(animState), animState, null);
            }
        }
        
        public Tween MoveAlongPath(List<Vector3> path)
        {
            if (path == null || path.Count < 2) return null;
            SetAnimationState(DummyAnimations.Running);
            float totalDistance = CalculateTotalDistance(path);
            float duration = totalDistance / Speed; // Time = Distance / Speed
            transform.DOKill();
            var tweenerCore = transform.DOPath(path.ToArray(), duration)
                .SetLookAt(0.01f) // Makes the character rotate along the path
                .SetEase(Ease.Linear);
            return tweenerCore;
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

        public void ResetRotation()
        {
            transform.DORotateQuaternion(_originalRotation, 0.2f)
                .SetEase(Ease.OutQuad); // Smoothly return to original rotation
        }
    }
}
