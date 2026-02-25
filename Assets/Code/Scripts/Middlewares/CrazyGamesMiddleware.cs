using CrazyGames;
using UnityEngine;
using UnityEngine.Events;

namespace XaviGames.Applications
{
    [CreateAssetMenu(fileName = "CrazyGamesMiddleware", menuName = "XaviGames/Middlewares/CrazyGames")]
    public class CrazyGamesMiddleware : Middleware
    {
        private bool _isFinished;

        public override bool IsFinished() => _isFinished;

        public override void Initialize(UnityAction onFinishCallback)
        {
            _isFinished = false;

            CrazySDK.Init(() =>
            {
                if (CrazySDK.IsInitialized)
                {
                    Debug.Log("[CrazyGames] SDK initialized successfully.");
                }
                else
                {
                    Debug.LogError("[CrazyGames] SDK failed to initialize.");
                }

                _isFinished = true;
                onFinishCallback?.Invoke();
            });
        }

        public override void Shutdown(UnityAction onFinishCallback)
        {
            _isFinished = true;
            onFinishCallback?.Invoke();
        }
    }
}