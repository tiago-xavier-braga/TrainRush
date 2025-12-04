using UnityEngine;

namespace XaviGames.Animation
{
    public class SpawnAnimation : MonoBehaviour
    {
        [SerializeField]
        private float _duration = 1.0f;

        [SerializeField]
        private LeanTweenType _leanTweenType;

        private int _id = -1;
        private Vector3 _initialScale;

        public void Animate(GameObject gameObject, Vector3 from, Vector3 to)
        {
            _initialScale = from;
            gameObject.transform.localScale = _initialScale;
            _id = LeanTween.scale(gameObject, to, _duration).setEase(_leanTweenType).id;
        }

        public void Cancel(GameObject gameObject)
        {
            LeanTween.cancel(_id);
            gameObject.transform.localScale = _initialScale;
        }
    }
}
