using System.Collections;
using TMPro;
using UnityEngine;
using XaviGames.Animation;
using XaviGames.SaveSystem;

namespace XaviGames.UI
{
    public class StatusBoxController : MonoBehaviour
    {
        [SerializeField]
        private IntModel _intModel;

        [SerializeField]
        private TextMeshProUGUI _textMeshProUGUI;

        [SerializeField]
        private SpawnAnimation _textSpawnAnimation;

        private Coroutine _textCoroutine;

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
