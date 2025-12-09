using UnityEngine;

namespace XaviGames.Utils
{
    public class DrawGizmosDebug : MonoBehaviour
    {
#if UNIY_EDITOR
        [SerializeField]
        private Color _color = Color.white;

        [SerializeField]
        private float _radius;

        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(Vector3.zero, _radius);
        }
#endif
    }
}
