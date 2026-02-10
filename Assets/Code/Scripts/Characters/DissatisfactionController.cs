using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XaviGames.Animation;
using XaviGames.Attributes;

namespace XaviGames.Characters
{
    public class DissatisfactionController : MonoBehaviour
    {
        [SerializeField]
        private CharacterAnimationController _characterAnimationController;

        [SerializeField]
        private SpawnAnimation _spawnAnimation;

        [SerializeField]
        private LookAtPlayer _lookAtPlayer;

        [SerializeField]
        private float _maxTime;

        [SerializeField]
        private float _animateTime;

        [SerializeField]
        private Vector3 _minCanvasScale;

        [SerializeField]
        private Vector3 _maxCanvasScale;

        [SerializeField]
        private List<Sprite> _reactSprites;

        [SerializeField]
        private Image _reactImage;

        [SerializeField]
        [ReadOnly]
        private float _currentTime;

        private Coroutine _animateCoroutine = null;

        private void OnDisable()
        {
            ResetCanvas();
        }

        private void Update()
        {
            if (_animateCoroutine != null)
            {
                return;
            }

            if (_characterAnimationController.CurrentState != CharactersState.Idle)
            {
                return;
            }

            _currentTime += Time.deltaTime;
            
            if (_currentTime >= _maxTime)
            {
                _animateCoroutine = StartCoroutine(AnimateCanvas());
            }
        }

        private IEnumerator AnimateCanvas()
        {
            int indexRandom = Random.Range(0, _reactSprites.Count - 1);
            _reactImage.sprite = _reactSprites[indexRandom];

            _lookAtPlayer.Enable();
            _spawnAnimation.Animate(_minCanvasScale, _maxCanvasScale);
            
            yield return new WaitForSeconds(_animateTime);

            ResetCanvas();
        }

        private void ResetCanvas()
        {
            _spawnAnimation.Despawn();
            _lookAtPlayer.Disable();
            _currentTime = 0;
            _animateCoroutine = null;
        }
    }
}