using System.Collections;
using UnityEngine;
using XaviGames.Audio;

namespace XaviGames.Interaction
{
    public class ButtonHoldAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private ButtonHoldController _controller;

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

        private float _timeAnimation;
        private Coroutine _blinkCoroutine;

        private void OnEnable()
        {
            _controller.OnEnterEvent.AddListener(HandleEnter);
            _controller.OnExitEvent.AddListener(HandleExit);
        }

        private void OnDisable()
        {
            _controller.OnEnterEvent.RemoveListener(HandleEnter);
            _controller.OnExitEvent.RemoveListener(HandleExit);
        }

        private void Update()
        {
            if (_controller.IsPlayerColliding)
            {
                IdleAnimation();
                return;
            }

            if (_isAnimationEnabled)
            {
                LoopAnimation();
            }
            else
            {
                IdleAnimation();
            }
        }

        public void SetAnimationEnabled(bool enable)
        {
            _isAnimationEnabled = enable;
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

        private void HandleEnter()
        {
            _timeAnimation = 0f;

            if (_enterSoundEffect != null)
            {
                _enterSoundEffect.PlayOneShort();
            }
        }

        private void HandleExit()
        {
            StopBlink();
            ResetFloorColor();

            _exitSoundEffect.PlayOneShort();
        }

        private void Animate(float yPosition)
        {
            Vector3 currentPosition = _containerTransform.position;
            currentPosition.y = yPosition;
            _containerTransform.position = currentPosition;
        }

        private void LoopAnimation()
        {
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
