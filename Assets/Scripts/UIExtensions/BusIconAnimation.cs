using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UIExtensions
{
    public class BusIconAnimation : MonoBehaviour
    {
        [SerializeField] private Image _loadingImage; // Reference to the image
        [SerializeField] private float _animationSpeed = 0.5f;
        [SerializeField] private float _scaleFactor = 1.2f; // Maximum scale multiplier

        private Coroutine _animationCoroutine;

        private void OnEnable()
        {
            // Start the animation when the object becomes active
            _animationCoroutine = StartCoroutine(AnimateLoadingImage());
        }

        private void OnDisable()
        {
            // Stop the animation when the object becomes inactive
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
        }

        private IEnumerator AnimateLoadingImage()
        {
            while (true)
            {
                yield return ScaleImage(Vector3.one * _scaleFactor, _animationSpeed);
                yield return ScaleImage(Vector3.one, _animationSpeed);
            }
        }

        private IEnumerator ScaleImage(Vector3 targetScale, float duration)
        {
            Vector3 startScale = _loadingImage.transform.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                _loadingImage.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _loadingImage.transform.localScale = targetScale; // Ensure final scale is applied
        }
    }
}