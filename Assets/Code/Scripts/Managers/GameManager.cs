using UnityEngine;

namespace XaviGames.Managers
{
    public class GameManager : MonoBehaviour
    {
        [field: SerializeField]
        public GameState GameState { get; private set; } = GameState.Running;

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetGameState(GameState newState)
        {
            GameState = newState;
        }
    }
}