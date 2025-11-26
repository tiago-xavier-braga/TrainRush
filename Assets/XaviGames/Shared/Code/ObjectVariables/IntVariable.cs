using UnityEngine;
using XaviGames.Attributes;

namespace XaviGames.ObjectVariables
{
    [CreateAssetMenu(fileName = "IntVariable", menuName = "XaviGames/ObjectVariables/IntVariable")]
    public class IntVariable : ScriptableObject
    {
        [SerializeField]
        private int _value;

        [field: SerializeField]
        [field: ReadOnly]
        public int Value;

        private void OnEnable()
        {
            Value = _value;
        }
    }
}