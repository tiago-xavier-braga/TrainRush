using UnityEngine;
using XaviGames.Attributes;

namespace XaviGames.SaveSystem
{
    public abstract class Model : ScriptableObject
    {
        [field: SerializeField]
        [field: ReadOnly]
        public string Key { get; protected set; }

        public abstract object GetValue();

        public abstract void Save(IDataStorage storage);
        public abstract void Load(IDataStorage storage);


#if UNITY_EDITOR
        private void OnValidate()
        {
            Key = name;
        }
#endif
    }
}
