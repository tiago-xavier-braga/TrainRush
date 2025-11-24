using UnityEngine;
using UnityEngine.Events;
using XaviGames.Applications;
using XaviGames.Attributes;

namespace XaviGames.Middlewares
{
    [CreateAssetMenu(fileName = "ScreenOrientationMiddleware", menuName = "XaviGames/Middlewares/ScreenOrientationMiddleware")]
    public class ScreenOrientationMiddleware : Middleware
    {
        [SerializeField]
        [ReadOnly]
        private bool _isFinished = false;

        public override bool IsFinished()
        {
            return _isFinished;
        }

        public override void Initialize(UnityAction onFinshCallback)
        {
            Debug.Log($"Screen Orientation: {Screen.orientation}");
            Shutdown(onFinshCallback);
        }

        public override void Shutdown(UnityAction onFinshCallback)
        {
            _isFinished = true;
            onFinshCallback?.Invoke();
        }
    }
}
