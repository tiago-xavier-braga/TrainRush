using System.Collections;
using TMPro;
using UnityEngine;
using XaviGames.Animation;
using XaviGames.SaveSystem;

namespace XaviGames.UI
{
    public class MoneyUIController : MonoBehaviour
    {
        [SerializeField]
        protected IntModel _intModel;

        [SerializeField]
        protected TextMeshProUGUI _textMeshProUGUI;

        [SerializeField]
        protected SpawnAnimation _textSpawnAnimation;

        protected Coroutine _textCoroutine;

        private void OnEnable()
        {
            _intModel.OnValueChanged += OnValueChanged;
        }

        private void OnDisable()
        {
            _intModel.OnValueChanged -= OnValueChanged;
        }

        private void Start()
        {
            _textMeshProUGUI.text = _intModel.Value.ToString();
        }

        private void OnValueChanged(int value)
        {
            if (_textCoroutine != null)
            {
                StopCoroutine(_textCoroutine);
            }

            _textCoroutine = StartCoroutine(TextAnimation());
        }

        private IEnumerator TextAnimation()
        {
            _textSpawnAnimation.Despawn();
            yield return new WaitForSeconds(_textSpawnAnimation.Duration);
            _textMeshProUGUI.text = _intModel.Value.ToString();
            _textSpawnAnimation.Spawn();
        }
    }
}
