using UnityEngine;
using UnityEngine.Events;

namespace XaviGames.Applications
{
    public abstract class Middleware : ScriptableObject
    {
        public abstract bool IsFinished();

        public abstract void Initialize(UnityAction onFinshCallback);

        public abstract void Shutdown(UnityAction onFinshCallback);
    }
}
