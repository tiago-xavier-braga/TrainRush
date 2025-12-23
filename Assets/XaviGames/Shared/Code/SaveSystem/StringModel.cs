using UnityEngine;
using UnityEngine.Events;

namespace XaviGames.SaveSystem
{
    [CreateAssetMenu(fileName = "Model", menuName = "XaviGames/SaveSystem/StringModel")]
    public class StringModel : ModelType<string>
    {
        public UnityAction<string> OnValueChanged;

        public void SetValue(string value)
        {
            UpdateValue(value);
            OnValueChanged?.Invoke(value);
        }
    }
}
