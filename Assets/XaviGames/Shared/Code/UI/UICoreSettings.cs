using UnityEngine;

namespace XaviGames.UI
{
    [CreateAssetMenu(fileName = "UICoreSettings", menuName = "Xavi Games/Ui/UICoreSettings")]
    public class UICoreSettings : ScriptableObject
    {
        [field: Header("Canvas Group Controller")]
        [field: SerializeField]
        public float EnableCanvasScale { get; private set; } = 1f;

        [field: SerializeField]
        public float DisableCanvasScale { get; private set; } = 0.8f;

        [field: SerializeField]
        public float AnimationDuration { get; private set; } = 0.5f;

        [field: Header("Button Extensions")]
        [field: SerializeField]
        public AudioClip ButtonReleaseSound;
        
        [field: SerializeField]
        public AudioClip ButtonClickSound;
    }
}
