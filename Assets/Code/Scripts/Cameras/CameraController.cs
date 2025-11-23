using UnityEngine;
using XaviGames.Attributes;

namespace XaviGames.Cameras
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _offset = new Vector3(-10f, 10f, -10f);

        [SerializeField]
        private Transform _target;

        private void LateUpdate()
        {
            if (_target == null)
            {
                return;
            }

            Vector3 targetPosition = _target.position + _offset;
            transform.position = targetPosition;

            transform.LookAt(_target.position);
        }
    }
}
