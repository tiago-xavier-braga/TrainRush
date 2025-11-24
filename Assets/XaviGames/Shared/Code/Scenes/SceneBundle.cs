using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XaviGames.Scenes
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "SceneBundle_", menuName = "XaviGames/Scenes/SceneBundle")]
    public class SceneBundle : ScriptableObject
    {
        [field: Space]
        [field: Header("Scenes References")]
        [field: SerializeField]
        public SceneReference SceneSingle { get; private set; }

        [field: Space]
        [field: SerializeField]
        public List<SceneReference> ScenesAdditives { get; private set; }

        public async void LoadScenesAsync(bool reloadSingleScene = true, Action<float> onTotalProgress = null)
        {
            if (!SceneManager.GetSceneByName(SceneSingle.SceneName).isLoaded || reloadSingleScene)
            {
                var mainSceneLoad = SceneManager.LoadSceneAsync(SceneSingle.SceneName, LoadSceneMode.Single);

                while (!mainSceneLoad.isDone)
                {
                    await Task.Yield();
                }
            }

            var additiveOperations = new List<(string sceneName, AsyncOperation op)>();

            foreach (var sceneRef in ScenesAdditives)
            {
                if (SceneManager.GetSceneByName(sceneRef.SceneName).isLoaded)
                {
                    await SceneManager.UnloadSceneAsync(sceneRef.SceneName);
                }
                var loadOp = SceneManager.LoadSceneAsync(sceneRef.SceneName, LoadSceneMode.Additive);
                additiveOperations.Add((sceneRef.SceneName, loadOp));
            }

            while (additiveOperations.Any(pair => !pair.op.isDone))
            {
                float totalProgress = 0f;

                foreach (var (sceneName, op) in additiveOperations)
                {
                    totalProgress += op.progress;
                }

                float averageProgress = additiveOperations.Count > 0 ? totalProgress / additiveOperations.Count : 1f;
                Debug.Log($"Loading Scenes Progress: {averageProgress * 100f}%");
                onTotalProgress?.Invoke(averageProgress);

                await Task.Yield();
            }

            onTotalProgress?.Invoke(1f);
        }
    }
}
