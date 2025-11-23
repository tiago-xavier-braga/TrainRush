using UnityEngine;
using XaviGames.Attributes;

namespace XaviGames.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupController : MonoBehaviour
    {
        [SerializeField]
        private UICoreSettings _uiCoreSettings;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private bool _scaleOnTransition = true;

        [SerializeField]
        private bool _isEnabled = false;

        [Button("Enable", true)]
        public virtual void EnableCanvas()
        {
            if (_isEnabled)
            {
                return;
            }

            LeanTween.cancel(gameObject);
            LeanTween.alphaCanvas(_canvasGroup, 1f, _uiCoreSettings.AnimationDuration)
                .setEase(LeanTweenType.easeInOutQuad);

            if (_scaleOnTransition)
            {
                LeanTween.scale(gameObject, Vector3.one * _uiCoreSettings.EnableCanvasScale, _uiCoreSettings.AnimationDuration)
                    .setEase(LeanTweenType.easeInOutQuad);
            }

            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _isEnabled = true;
        }

        [Button("Disable", true)]
        public virtual void DisableCanvas()
        {
            if (!_isEnabled)
            {
                return;
            }

            LeanTween.cancel(gameObject);

            LeanTween.alphaCanvas(_canvasGroup, 0f, _uiCoreSettings.AnimationDuration)
                .setEase(LeanTweenType.easeInOutQuad);

            if (_scaleOnTransition)
            {
                LeanTween.scale(gameObject, Vector3.one * _uiCoreSettings.DisableCanvasScale, _uiCoreSettings.AnimationDuration)
                .setEase(LeanTweenType.easeInOutQuad);
            }

            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _isEnabled = false;
        }
    }
}

