using UnityEngine;

namespace XaviGames.SaveSystem
{
    [CreateAssetMenu(fileName = "Model", menuName = "XaviGames/SaveSystem/Model")]
    public class Model : ScriptableObject
    {
        [field: SerializeField]
        public string Key { get; private set; }

        [field: SerializeField]
        public DataType Type { get; private set; }

        public object Value;
    }
}
