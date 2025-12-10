using UnityEngine;
using XaviGames.Attributes;
using XaviGames.ObjectVariables;
using XaviGames.SaveSystem;

namespace XaviGames.EconomySystem
{
    public class EconomyController : MonoBehaviour
    {
        [Header("Default Values")]
        [SerializeField]
        private int _defaultPlayerCoins;

        [SerializeField]
        private int _defaultPlayerDiamongs;

        [Header("Variables")]
        [SerializeField]
        private IntVariable _playerCoinsVariable;

        [SerializeField]
        private IntVariable _playerDiamondsVariable;

        [Header("Models")]
        [SerializeField]
        private Model _playerCoinsModel;

        [SerializeField]
        private Model _playerDiamondsModel;

        [Header("Debug")]
        [SerializeField]
        [ReadOnly]
        private int _debugCoins = 0;

        [SerializeField]
        [ReadOnly]
        private int _debugDiamonds = 0;

        private void Start()
        {
            if (_playerCoinsModel.Value != null)
            {
                _playerCoinsVariable.Value = (int)_playerCoinsModel.Value;
            }
            else
            {
                _playerCoinsModel.Value = _defaultPlayerCoins;
            }

            if (_playerDiamondsModel.Value != null)
            {
                _playerDiamondsVariable.Value = (int)_playerDiamondsModel.Value;
            }
            else
            {
                _playerDiamondsVariable.Value = _defaultPlayerDiamongs;
            }
        }

        private void Update()
        {
            _debugCoins = Mathf.RoundToInt(_playerCoinsVariable.Value);
            _debugDiamonds = Mathf.RoundToInt(_playerDiamondsVariable.Value);
        }

        public void AddCoins(int amount) => AddValue(_playerCoinsVariable, _playerCoinsModel, amount);

        public void AddDiamonds(int amount) => AddValue(_playerDiamondsVariable, _playerDiamondsModel, amount);

        public void RemoveCoins(int amount) => RemoveValue(_playerCoinsVariable, _playerCoinsModel, amount);

        public void RemoveDiamonds(int amount) => RemoveValue(_playerDiamondsVariable, _playerDiamondsModel, amount);
        
        private void AddValue(IntVariable variable, Model model, int amount)
        {
            variable.Value += amount;
            model.Value = variable.Value;
        }

        private void RemoveValue(IntVariable variable, Model model, int amount)
        {
            if (variable.Value - amount < 0)
            {
                amount = variable.Value;
            }

            variable.Value -= amount;
            model.Value = variable.Value;
        }

    }
}
