using UnityEngine;

namespace XaviGames.Utils
{
    public class DrawGizmosDebug : MonoBehaviour
    {
        [SerializeField]
        private Color _color = Color.white;

        [SerializeField]
        private float _radius;

        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, _radius);
        }
    }
}
