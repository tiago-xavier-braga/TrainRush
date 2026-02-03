using UnityEngine;

namespace XaviGames.Interaction
{
    public class TrafficLight : MonoBehaviour
    {
        [SerializeField]
        private GameObject _lightOn;

        [SerializeField] 
        private GameObject _lightOff;

        public void Enable()
        {
            SetState(true);
        }

        public void Disable()
        {
            SetState(false);
        }

        private void SetState(bool isOn)
        {
            _lightOn.SetActive(isOn);
            _lightOff.SetActive(!isOn);
        }
    }
}
