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
            _startMiddlewares.ForEach(middleware => middleware.Initialize
            (
                () => 
                { 
                    OnActionFinished(_startMiddlewares, _gameSceneBundle.LoadScenesAsync);
                })
            );
        }

        [Button(true)]
        public void ShutdownApplication()
        {
            _finishMiddlewares.ForEach(middleware => middleware.Initialize
            (
                () =>
                {
                    OnActionFinished(_finishMiddlewares, ApplicationQuit);
                })
            );
        }

        private void OnActionFinished(List<Middleware> middlewares, UnityAction nextAction = null)
        {
            if (middlewares.TrueForAll(middleware => middleware.IsFinished()))
            {
                nextAction?.Invoke();
            }
        }

        private void ApplicationQuit()
        {
            UnityEngine.Application.Quit();

            if (UnityEngine.Application.isEditor)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }
    }
}
