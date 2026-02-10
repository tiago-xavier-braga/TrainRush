using UnityEngine;
using XaviGames.Attributes;

namespace XaviGames.Animation
{
    public class LookAtPlayer : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private bool _isActivated = false;

        private void Update()
        {
            if (!_isActivated)
            {
                return;
            }

            Vector3 directionToCamera = Camera.main.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(-directionToCamera);
        }

        public void Enable()
        {
            _isActivated = true;
        }

        public void Disable()
        {
            _isActivated = false;
        }

    }
}
