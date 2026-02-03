using UnityEngine;

namespace XaviGames.Animation
{
    public class LeverAnimation : MonoBehaviour
    {
        [SerializeField]
        private Transform _transform;

        [SerializeField]
        private float _animationDuration;

        [SerializeField]
        private LeanTweenType _leanTweenType;

        [SerializeField]
        private Vector3 _defaultRotate;

        [SerializeField]
        private Vector3 _activatedRotate;

        private int _leanTweenId = -1;

        public void EnableAnimation()
        {
            Animate(_defaultRotate, _activatedRotate);
        }

        public void ResetAnimation()
        {
            Animate(_activatedRotate, _defaultRotate);
        }

        private void Animate(Vector3 from, Vector3 to )
        {
            if (_leanTweenId != -1)
            {
                LeanTween.cancel(_leanTweenId);
            }

            _transform.eulerAngles = from;

            _leanTweenId = LeanTween.rotate(_transform.gameObject, to, _animationDuration)
                .setEase(_leanTweenType).id;
        }
    }
}
