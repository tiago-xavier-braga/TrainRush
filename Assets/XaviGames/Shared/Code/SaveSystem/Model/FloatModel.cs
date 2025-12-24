using UnityEngine;
using UnityEngine.Events;

namespace XaviGames.SaveSystem
{
    [CreateAssetMenu(fileName = "Model", menuName = "XaviGames/SaveSystem/FloatModel")]
    public class FloatModel : ModelType<float>
    {
        public UnityAction<float> OnValueChanged;

        public void SetValue(float value)
        {
            UpdateValue(value);
            OnValueChanged?.Invoke(value);
        }
    }
}
