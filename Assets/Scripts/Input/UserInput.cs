using System;
using System.Collections.Generic;
using Core.Actors;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Input
{
    public class UserInput : MonoBehaviour
    {
        [SerializeField] private Camera _cam;
        [SerializeField] private EventSystem _eventSystem;

        private static bool _isInputOn = true;
        public static event Action<Dummy> OnDummyTouched;
        private IA_User _iaUser;
    
        private void Awake()
        {
            _iaUser = new IA_User(); // Instantiate the input actions class
            _iaUser.Level.Enable(); // Enable the specific action map
            _iaUser.Level.Touch.performed += TouchItemNotifier; // Subscribe to the action
        }

        private void OnDestroy()
        {
            _iaUser.Level.Disable();
            _iaUser.Level.Touch.performed -= TouchItemNotifier;
            OnDummyTouched = null;
        }

        public static void SetInputState(bool isInputOn)
        {
            _isInputOn = isInputOn;
        }

        private bool IsPointerOverUIObject()
        {
            // Create PointerEventData for the current event system
            PointerEventData eventData = new PointerEventData(_eventSystem);

#if UNITY_EDITOR || UNITY_STANDALONE
            // Use mouse position for PC builds and the Unity editor
            eventData.position = UnityEngine.Input.mousePosition;
#else
        // Use touch position for mobile devices
        if (UnityEngine.Input.touchCount > 0)
            eventData.position = UnityEngine.Input.GetTouch(0).position;
        else
            return false;
#endif

            // Perform a raycast and check if any UI elements were hit
            List<RaycastResult> results = new List<RaycastResult>();
            _eventSystem.RaycastAll(eventData, results);

            // Return true if any UI elements were hit, false otherwise
            return results.Count > 0;
        }
    
        private void TouchItemNotifier(InputAction.CallbackContext context)
        {
            if (IsPointerOverUIObject() || !_isInputOn)
                return;
            Ray ray = _cam.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f) && hit.transform.TryGetComponent<Dummy>(out var dummy))
            {
                OnDummyTouched?.Invoke(dummy);
            }
        }
    }
}
