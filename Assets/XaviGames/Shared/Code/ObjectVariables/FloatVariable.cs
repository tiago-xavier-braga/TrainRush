using UnityEngine;
using XaviGames.Attributes;

namespace XaviGames.ObjectVariables
{
    [CreateAssetMenu(fileName = "FloatVariable", menuName = "XaviGames/FloatVariable")]
    public class FloatVariable : ScriptableObject
    {
        [SerializeField]
        private float _value;

        [field: SerializeField]
        [field: ReadOnly]
        public float Value { get; private set; }

        private void OnEnable()
        {
            Value = _value;
        }
    }
}
