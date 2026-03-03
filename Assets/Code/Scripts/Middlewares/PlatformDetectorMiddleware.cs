using CrazyGames;
using UnityEngine;
using UnityEngine.Events;
using XaviGames.Attributes;

namespace XaviGames.Applications
{
    [CreateAssetMenu(fileName = "PlatformDetectorMiddleware", menuName = "XaviGames/Middlewares/Platform Detector")]
    public class PlatformDetectorMiddleware : Middleware
    {
        [Header("Platform Info")]
        [field: SerializeField]
        [field: ReadOnly]
        public bool IsMobile { get; private set; }

        [field: SerializeField]
        [field: ReadOnly]
        public bool IsTablet { get; private set; }

        [field: SerializeField]
        [field: ReadOnly]
        public bool IsDesktop { get; private set; }
        
        [field: SerializeField]
        [field: ReadOnly]
        public bool IsWeb { get; private set; }

        [field: SerializeField]
        [field: ReadOnly]      
        public string CountryCode { get; private set; }
        
        [field: SerializeField]
        [field: ReadOnly]
        public string Locale { get; private set; }
        
        [field: SerializeField]
        [field: ReadOnly]
        public string BrowserName { get; private set; }
        
        [field: SerializeField]
        [field: ReadOnly]
        public string BrowserVersion { get; private set; }
        
        [field: SerializeField]
        [field: ReadOnly]
        public string OsName { get; private set; }
        
        [field: SerializeField]
        [field: ReadOnly]
        public string OsVersion { get; private set; }
        
        [field: SerializeField]
        [field: ReadOnly]
        public string DeviceType { get; private set; }
        
        [field: SerializeField]
        [field: ReadOnly]
        public string ApplicationType { get; private set; }

        private bool _isFinished;

        public override void Initialize(UnityAction onFinishCallback)
        {
            _isFinished = false;

            var systemInfo = CrazySDK.User.SystemInfo;

            CountryCode = systemInfo.countryCode;
            Locale = systemInfo.locale;

            BrowserName = systemInfo.browser.name;
            BrowserVersion = systemInfo.browser.version;

            OsName = systemInfo.os.name;
            OsVersion = systemInfo.os.version;

            DeviceType = systemInfo.device.type;  // "desktop" | "tablet" | "mobile"
            ApplicationType = systemInfo.applicationType; // "google_play_store" | "apple_store" | "pwa" | "web"

            IsMobile = DeviceType == "mobile";
            IsTablet = DeviceType == "tablet";
            IsDesktop = DeviceType == "desktop";
            IsWeb = ApplicationType == "web" || ApplicationType == "pwa";

            LogSystemInfo();

            _isFinished = true;
            onFinishCallback?.Invoke();
        }

        public override void Shutdown(UnityAction onFinishCallback)
        {
            _isFinished = false;
            ResetState();

            _isFinished = true;
            onFinishCallback?.Invoke();
        }

        public override bool IsFinished() => _isFinished;

        private void LogSystemInfo()
        {
            Debug.Log($"[PlatformDetector] Country: {CountryCode} | Locale: {Locale}");
            Debug.Log($"[PlatformDetector] Browser: {BrowserName} {BrowserVersion}");
            Debug.Log($"[PlatformDetector] OS: {OsName} {OsVersion}");
            Debug.Log($"[PlatformDetector] Device: {DeviceType} | App: {ApplicationType}");
            Debug.Log($"[PlatformDetector] IsMobile={IsMobile} | IsTablet={IsTablet} | IsDesktop={IsDesktop} | IsWeb={IsWeb}");
        }

        private void ResetState()
        {
            IsMobile = IsTablet = IsDesktop = IsWeb = false;
            CountryCode = Locale = BrowserName = BrowserVersion = string.Empty;
            OsName = OsVersion = DeviceType = ApplicationType = string.Empty;
        }
    }
}