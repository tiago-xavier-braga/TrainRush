using UnityEngine;
using System;
using UnityEditor;

namespace XaviGames.Scenes
{
    [Serializable]
    public class SceneReference
    {
#if UNITY_EDITOR
        [SerializeField]
        private SceneAsset _sceneAsset;
#endif

        [field: SerializeField]
        public string SceneName { get; private set; }
    }
}
