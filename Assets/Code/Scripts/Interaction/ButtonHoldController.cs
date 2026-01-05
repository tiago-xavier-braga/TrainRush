using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XaviGames.Attributes;

namespace XaviGames.Interaction
{
    public class ButtonHoldController : MonoBehaviour
    {
        [Header("Logic Parameters")]
        [SerializeField]
        private Collider _playerCollider;

        [SerializeField]
        private float _timeToHold;

        [Header("Canvas References")]
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private Image _iconImage;

        [SerializeField]
        private TextMeshProUGUI _textMeshPro;

        [Header("Debug")]
        [SerializeField]
        [ReadOnly]
        private float _holdTime;

        [SerializeField]
        [ReadOnly]
        private bool _isPlayerColliding;

        [Header("Unity Events")]
        [SerializeField]
        public UnityEvent OnEnterEvent;

        [Space]
        [SerializeField]
        public UnityEvent OnStayEvent;

        [Space]
        [SerializeField]
        public UnityEvent OnHoldEvent;

        [Space]
        [SerializeField]
        public UnityEvent OnExitEvent;

        public bool IsPlayerColliding => _isPlayerColliding;

        private void Update()
        {
            if (_isPlayerColliding)
            {
                PlayerHolding();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other != _playerCollider)
            {
                return;
            }

            _isPlayerColliding = true;
            _holdTime = 0f;

            OnEnterEvent?.Invoke();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other != _playerCollider)
            {
                return;
            }

            OnStayEvent?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other != _playerCollider)
            {
                return;
            }

            _isPlayerColliding = false;
            _holdTime = 0f;

            OnExitEvent?.Invoke();
        }

        public void ShowCanvas()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
        }

        public void HideCanvas()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }

        public void SetNewText(string text)
        {
            if (_textMeshPro == null)
            {
                return;
            }

            _textMeshPro.text = text;
        }

        public void SetNewIcon(Sprite sprite)
        {
            if (_iconImage == null)
            {
                return;
            }

            _iconImage.sprite = sprite;
        }

        private void PlayerHolding()
        {
            _holdTime += Time.deltaTime;

            if (_holdTime < _timeToHold)
            {
                return;
            }

            _holdTime = 0f;

            OnHoldEvent?.Invoke();
        }
    }
}
