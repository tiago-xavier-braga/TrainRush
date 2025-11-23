using TMPro;
using UnityEngine;

namespace XaviGames.UI
{
    public class FpsDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _fpsText;

        private float elapsed;
        private int frames;

        private void Start()
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            gameObject.SetActive(true);
#else
            gameObject.SetActive(false);
#endif
        }

        private void Update()
        {
            elapsed += Time.unscaledDeltaTime;
            frames++;

            if (elapsed >= 0.5f)
            {
                float fps = frames / elapsed;
                _fpsText.text = "FPS: " + Mathf.RoundToInt(fps);
                elapsed = 0f;
                frames = 0;
            }
        }
    }
}
