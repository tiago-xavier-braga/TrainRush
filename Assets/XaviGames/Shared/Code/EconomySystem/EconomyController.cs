using UnityEngine;
using XaviGames.Attributes;
using XaviGames.ObjectVariables;
using XaviGames.SaveSystem;

namespace XaviGames.EconomySystem
{
    public class EconomyController : MonoBehaviour
    {
        [Header("Models")]
        [SerializeField]
        private IntModel _playerCoinsModel;

        [SerializeField]
        private IntModel _playerDiamondsModel;

        [SerializeField]
        private DataController _dataController;

        [Header("Debug")]
        [SerializeField]
        [ReadOnly]
        private int _debugCoins = 0;

        [SerializeField]
        [ReadOnly]
        private int _debugDiamonds = 0;

        [SerializeField]
        private int numberToAddForTest = 100;

        private void Update()
        {
            _debugCoins = _playerCoinsModel.Value;
            _debugDiamonds = _playerDiamondsModel.Value;
        }

        public void AddCoins(int amount) => AddValue(_playerCoinsModel, amount);

        public void AddDiamonds(int amount) => AddValue(_playerDiamondsModel, amount);

        public void RemoveCoins(int amount) => RemoveValue(_playerCoinsModel, amount);

        public void RemoveDiamonds(int amount) => RemoveValue(_playerDiamondsModel, amount);

        private void AddValue(IntModel model, int amount)
        {
            int newValue = model.Value + amount;
            model.SetValue(newValue);
            _dataController.SaveModel(model);
        }

        private void RemoveValue(IntModel model, int amount)
        {
            if (model.Value - amount < 0)
            {
                return;
            }

            int newValue = model.Value - amount;
            model.SetValue(newValue);
            _dataController.SaveModel(model);
        }

        [Button]
        private void AddCoinTest()
        {
            AddValue(_playerCoinsModel, numberToAddForTest);
        }

        [Button]
        private void RemoveCoinTest()
        {
            AddValue(_playerCoinsModel, numberToAddForTest);
        }
    }
}
