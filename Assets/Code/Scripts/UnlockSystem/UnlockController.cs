using System.Collections;
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

        [Header("Canvas References")]
        [SerializeField]
        private UnityEngine.UI.Image _iconImage;

        [SerializeField]
        private TextMeshProUGUI _priceTextMeshPro;

        [Header("Animation")]
        [SerializeField]
        private bool _isAnimationEnabled = false;

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

        [Header("Color Animation")]
        [SerializeField]
        private MeshRenderer _floorMarkingRenderer;

        [SerializeField]
        private float _colorAnimationDuration;

        [SerializeField]
        private float _blinkSpeed;

        [SerializeField]
        private Color _defaultColor;

        [SerializeField]
        private Color _failureColor;

        [SerializeField]
        private Color _successColor;

        [Header("Sound Effect")]
        [SerializeField]
        private SoundEffect _enterSoundEffect;

        [SerializeField]
        private SoundEffect _exitSoundEffect;

        [SerializeField]
        private SoundEffect _successSoundEffect;

        [SerializeField]
        private SoundEffect _failureSoundEffect;

        [Header("Debug")]
        [SerializeField]
        [ReadOnly]
        private float _holdTime;

        [Header("Unity Events")]
        [Space]
        [SerializeField]
        private UnityEvent OnHoldFinished;

        private float _timeAnimation;
        private bool _isPlayerColliding;
        private Coroutine _blinkCoroutine;

        private void Update()
        {
            if (!_isAnimationEnabled)
            {
                IdleAnimation();
            }

            if (_isPlayerColliding)
            {
                PlayerHolding();
                return;
            }

            LoopAnimation();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other != _playerCollider)
            {
                return;
            }

            _isPlayerColliding = true;
            _timeAnimation = 0f;
            _enterSoundEffect?.PlayOneShort();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other != _playerCollider)
            {
                return;
            }

            _isPlayerColliding = false;
            _holdTime = 0f;

            StopBlink();
            ResetFloorColor();

            _exitSoundEffect?.PlayOneShort();
        }

        public void SuccessUnlocked()
        {
            _successSoundEffect.PlayOneShort();
            PlayBlink(_successColor);
        }

        public void FailureUnlocked()
        {
            _failureSoundEffect.PlayOneShort();
            PlayBlink(_failureColor);
        }

        public void SetNewPrice(string price)
        {
            _priceTextMeshPro.text = price;
        }

        public void SetNewIcon(Sprite sprite)
        {
            _iconImage.sprite = sprite;
        }

        public void EnableAnimation(bool enable)
        {
            _isAnimationEnabled = enable;
        }

        private void Animate(float yPosition)
        {
            Vector3 currentPosition = _containerTransform.position;
            currentPosition.y = yPosition;
            _containerTransform.position = currentPosition;
        }

        private void LoopAnimation()
        {
            if (!_isAnimationEnabled)
            {
                return;
            }

            _timeAnimation += Time.deltaTime * _animationSpeed;

            float range = _maxYPosition - _minYPosition;
            float newY = _minYPosition + Mathf.PingPong(_timeAnimation, range);

            Animate(newY);
        }

        private void IdleAnimation()
        {
            float yPosition = _containerTransform.position.y;
            yPosition = Mathf.Max(yPosition - Time.deltaTime * _holdAnimationSpeed, _minYPosition);

            Animate(yPosition);
        }

        private void PlayerHolding()
        {
            if (_isAnimationEnabled)
            {
                IdleAnimation();
            }

            _holdTime += Time.deltaTime;

            if (_holdTime < _timeToUnlock)
            {
                return;
            }

            _holdTime = 0f;
            OnHoldFinished?.Invoke();
        }

        private void PlayBlink(Color blinkColor)
        {
            StopBlink();
            _blinkCoroutine = StartCoroutine(BlinkFloorSmooth(blinkColor));
        }

        private void StopBlink()
        {
            if (_blinkCoroutine != null)
            {
                StopCoroutine(_blinkCoroutine);
            }
            _blinkCoroutine = null;
        }

        private void ResetFloorColor()
        {
            _floorMarkingRenderer.material.color = _defaultColor;
        }

        private IEnumerator BlinkFloorSmooth(Color blinkColor)
        {
            float elapsed = 0f;
            Material mat = _floorMarkingRenderer.material;

            while (elapsed < _colorAnimationDuration)
            {
                float time = Mathf.Abs(Mathf.Sin(elapsed * _blinkSpeed));
                mat.color = Color.Lerp(_defaultColor, blinkColor, time);

                elapsed += Time.deltaTime;
                yield return null;
            }

            mat.color = _defaultColor;
            _blinkCoroutine = null;
        }
    }
}
