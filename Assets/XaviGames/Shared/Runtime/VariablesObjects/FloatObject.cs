using UnityEngine;

namespace XaviGames.Shared
{
    [CreateAssetMenu(fileName = "_FloatObject", menuName = "Xavi Games/Variables Objects/Float Object")]
    public class FloatObject : ScriptableObject
    {
        [SerializeField]
        private float _value;

        [field: SerializeField]
        [field: ReadOnly]
        public float Value { get; private set; }

        public void OnEnable()
        {
            Value = _value;
        }
        public void SetValue(float value)
        {
            Value = value;
        }
    }
}
