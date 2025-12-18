using TMPro;
using UnityEngine;
using UnityEngine.Events;
using XaviGames.Attributes;
using XaviGames.Audio;

namespace XaviGames.UnlockSystem 
{
    public class UnlockController : MonoBehaviour
    {
        [Header("Logic Parameters")]
        [SerializeField]
        private Collider _playerCollider;

        [SerializeField]
        private float _timeToUnlock;

        [SerializeField]
        private TextMeshProUGUI _priceTextMeshPro;

        [Header("Animation")]
        [SerializeField]
        private Transform _containerTransform;

        [SerializeField]
        private float _animationSpeed;

        [SerializeField]
        private float _holdAnimationSpeed;

        [SerializeField]
        private float _minYPosition;

        [SerializeField]
        private float _maxYPosition;

        [Header("Sound Effect")]
        [SerializeField]
        private SoundEffect _enterSoundEffect;

        [SerializeField]
        private SoundEffect _exitSoundEffect;

        [Header("Debug")]
        [SerializeField]
        [ReadOnly]
        private float _holdTime;

        [Header("Unity Events")]
        [Space]
        [SerializeField]
        private UnityEvent OnHoldFinished;

        private float _timeAnimation;
        private bool _isPlayerColliding = false;

        private void Update()
        {
            if (_isPlayerColliding)
            {
                float yPosition = _containerTransform.position.y;

                yPosition = Mathf.Max(yPosition - Time.deltaTime * _holdAnimationSpeed, _minYPosition);

                Animate(yPosition);
                _holdTime += Time.deltaTime;

                if (_holdTime >= _timeToUnlock)
                {
                    _holdTime = 0;
                    OnHoldFinished?.Invoke();
                }
            }
            else
            {
                _timeAnimation += Time.deltaTime * _animationSpeed;
                float newY = Mathf.PingPong(_timeAnimation, _maxYPosition - _minYPosition);
                Animate(newY);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other != _playerCollider)
            {
                return;
            }

            _isPlayerColliding = true;
            _timeAnimation = 0f;
            _enterSoundEffect.PlayOneShort();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other != _playerCollider)
            {
                return;
            }

            _isPlayerColliding = false;
            _holdTime = 0;
            _exitSoundEffect.PlayOneShort();
        }

        public void SetNewPrice(string price)
        {
            if (_priceTextMeshPro == null)
            {
                return;
            }

            _priceTextMeshPro.text = price;
        }

        private void Animate(float yPosition)
        {
            Vector3 currentPosition = _containerTransform.position;
            currentPosition.y = yPosition;
            _containerTransform.position = currentPosition;
        }
    }
}