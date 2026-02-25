using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XaviGames.Attributes;
using XaviGames.Scenes;

namespace XaviGames.Applications
{
    public class ApplicationManager : MonoBehaviour
    {
        [Header("Middlewares")]
        [SerializeField]
        private List<Middleware> _startMiddlewares;
        [Space(8f)]
        [SerializeField]
        private List<Middleware> _finishMiddlewares;

        [Header("Game Settings")]
        [SerializeField]
        private SceneBundle _gameSceneBundle;

        public static ApplicationManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            InitializeApplication();
        }

        [Button(true)]
        public void InitializeApplication()
        {
            ExecuteMiddlewaresSequentially(_startMiddlewares, 0, _gameSceneBundle.LoadScenesAsync);
        }

        [Button(true)]
        public void ShutdownApplication()
        {
            ExecuteMiddlewaresSequentially(_finishMiddlewares, 0, ApplicationQuit);
        }

        private void ExecuteMiddlewaresSequentially(List<Middleware> middlewares, int index, UnityAction finalAction)
        {
            if (index >= middlewares.Count)
            {
                finalAction?.Invoke();
                return;
            }

            middlewares[index].Initialize(() =>
            {
                if (middlewares[index].IsFinished())
                {
                    ExecuteMiddlewaresSequentially(middlewares, index + 1, finalAction);
                }
            });
        }

        private void ApplicationQuit()
        {
#if UNITY_EDITOR
            UnityEngine.Application.Quit();
            if (UnityEngine.Application.isEditor)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
#endif
        }
    }
}