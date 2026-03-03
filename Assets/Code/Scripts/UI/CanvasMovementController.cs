using UnityEngine;
using XaviGames.Applications;

namespace XaviGames.UI
{
    public class CanvasMovementController : MonoBehaviour
    {
        [SerializeField] 
        private PlatformDetectorMiddleware _platformMiddleware;
        
        [SerializeField] 
        private GameObject _joystickCanvas;

        private void Start()
        {
            _joystickCanvas.SetActive(_platformMiddleware.IsMobile);
        }
    }
}