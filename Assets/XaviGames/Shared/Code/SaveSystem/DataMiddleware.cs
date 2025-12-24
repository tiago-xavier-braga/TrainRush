using UnityEngine;
using UnityEngine.Events;
using XaviGames.Applications;
using XaviGames.Attributes;

namespace XaviGames.SaveSystem
{
    [CreateAssetMenu(fileName = "DataMiddleware", menuName = "XaviGames/Middlewares/DataMiddleware")]
    public class DataMiddleware : Middleware
    {
        private enum Option
        {
            Save,
            Load,
        }

        [SerializeField]
        [ReadOnly]
        private bool _isFinished = false;

        [SerializeField]
        private Option _option = Option.Save;

        [SerializeField]
        private DataController _saveSystem;

        public override bool IsFinished()
        {
            return _isFinished;
        }
        
        public override void Initialize(UnityAction onFinshCallback)
        {
            if (_option == Option.Save)
            {
                _saveSystem.SaveAllModels();
            }
            else if (_option == Option.Load)
            {
                _saveSystem.LoadAllModels();
            }

            Shutdown(onFinshCallback);
        }

        public override void Shutdown(UnityAction onFinshCallback)
        {
            _isFinished = true;
            onFinshCallback?.Invoke();
        }
    }
}
