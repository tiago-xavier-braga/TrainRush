using UnityEngine;
using UnityEngine.EventSystems;

namespace XaviGames.UI
{
    public class ButtonExtension : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [SerializeField]
        private UICoreSettings _uiCoreSettings;

        [SerializeField]
        private AudioData _audioData;

        [SerializeField]
        private AudioSource _audioSource;

        public void OnPointerClick(PointerEventData eventData)
        {
            _audioSource.PlayOneShot(_uiCoreSettings.ButtonClickSound, _audioData.Volume);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _audioSource.PlayOneShot(_uiCoreSettings.ButtonReleaseSound, _audioData.Volume);
        }
    }
}
