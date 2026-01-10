using TMPro;
using UnityEngine;
using XaviGames.Attributes;

namespace XaviGames.Animation
{
    public class CameraFacingTextAnimation : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private TextMeshProUGUI _textMeshPro;

        [SerializeField]
        private Transform _cameraTransform;

        [SerializeField]
        private float _distanceY;

        [SerializeField]
        private float _duration;

        [SerializeField] 
        private Vector3 _scaleEnd;

        [SerializeField]
        private LeanTweenType _easeType;

        private bool _isAnimating = false;

        private void Update()
        {
            if (!_isAnimating)
            {
                return;
            }

            Vector3 directionToCamera = _cameraTransform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(-directionToCamera);
        }

        public void SetText(string text)
        {
            _textMeshPro.text = text;
        }

        [Button]
        public void Enable()
        {
            if (_isAnimating)
            {
                return;
            }

            _isAnimating = true;
            _canvasGroup.alpha = 1f;
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = startPosition + new Vector3(0f, _distanceY, 0f);

            transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, _scaleEnd, _duration).setEase(_easeType);

            LeanTween
                .move(gameObject, targetPosition, _duration)
                .setEase(_easeType)
                .setOnComplete(() =>
                {
                    _canvasGroup.alpha = 0f;
                    transform.position = startPosition;
                    _isAnimating = false;
                });
        }
    }
}
