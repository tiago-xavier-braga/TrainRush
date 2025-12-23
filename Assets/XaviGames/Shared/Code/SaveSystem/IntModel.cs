using UnityEngine;
using UnityEngine.Events;

namespace XaviGames.SaveSystem
{
    [CreateAssetMenu(fileName = "Model", menuName = "XaviGames/SaveSystem/IntModel")]
    public class IntModel : ModelType<int>
    {
        public UnityAction<int> OnValueChanged;

        public void SetValue(int value)
        {
            UpdateValue(value);
            OnValueChanged?.Invoke(value);
        }
    }
}

