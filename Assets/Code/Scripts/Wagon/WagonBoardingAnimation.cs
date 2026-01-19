using UnityEngine;

namespace XaviGames.Wagon
{
    public class WagonBoardingAnimation : MonoBehaviour
    {
        [SerializeField]
        private WagonSpawnController _wagonUpgradeController;

        [Header("Canvas Animation")]
        [SerializeField]
        private Transform _cameraTransform;

        [SerializeField]
        private Transform _canvasTransform;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [Header("Scale Animation")]
        [SerializeField]
        private GameObject _wagon;

        [SerializeField]
        private Vector3 _defaultScale = Vector3.one;

        [SerializeField]
        private Vector3 _expandedScale = Vector3.one;

        [SerializeField]
        private LeanTweenType _easeType;

        [SerializeField]
        private float _animationDuration = 0.5f;

        private int _leantweenId = -1;
        private bool _isCanvasVisible = false;


        private void Update()
        {
            if (!_isCanvasVisible)
            {
                return;
            }

            Vector3 directionToCamera = _cameraTransform.position - _canvasTransform.position;
            _canvasTransform.rotation = Quaternion.LookRotation(-directionToCamera);
        }

        public void UpdateBoardingVisuals(int boardedValue)
        {
            UpdateWagonScale(boardedValue);
            UpdateBoardingCanvas(boardedValue);
        }

        //TODO: Refactor this code 
        private void UpdateBoardingCanvas(int boardedValue)
        {
            //if (boardedValue >= _wagonUpgradeController.Capacity)
            //{
            //    LeanTween.alphaCanvas(_canvasGroup, 1f, _animationDuration)
            //        .setEase(_easeType);

            //    _isCanvasVisible = true;
            //}
            //else
            //{
            //    LeanTween.alphaCanvas(_canvasGroup, 0f, _animationDuration)
            //        .setEase(_easeType);

            //    _isCanvasVisible = false;
            //}
        }

        private void UpdateWagonScale(int boardedValue)
        {
            //if (_leantweenId != -1)
            //{
            //    LeanTween.cancel(_leantweenId);
            //    _leantweenId = -1;
            //}

            //int totalCapacity = _wagonUpgradeController.Capacity;
            //float scaleFactor = (boardedValue * (100f / totalCapacity)) / 100f;
            //Vector3 targetScale = Vector3.Lerp(_defaultScale, _expandedScale, scaleFactor);

            //_leantweenId = LeanTween.scale(_wagon, targetScale, _animationDuration)
            //    .setEase(_easeType)
            //    .setOnComplete(() => _leantweenId = -1)
            //    .id;
        }

    }
}