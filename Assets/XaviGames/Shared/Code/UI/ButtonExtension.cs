using UnityEngine;
using UnityEngine.EventSystems;
using AudioSettings = XaviGames.Audio.AudioSettings;

namespace XaviGames.UI
{
    public class ButtonExtension : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [SerializeField]
        private UICoreSettings _uiCoreSettings;

        [SerializeField]
        private AudioSettings _audioSettings;

        [SerializeField]
        private AudioSource _audioSource;

        public void OnPointerClick(PointerEventData eventData)
        {
            _audioSource.PlayOneShot(_uiCoreSettings.ButtonClickSound, _audioSettings.MasterVolume);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _audioSource.PlayOneShot(_uiCoreSettings.ButtonReleaseSound, _audioSettings.MasterVolume);
        }
    }
}
