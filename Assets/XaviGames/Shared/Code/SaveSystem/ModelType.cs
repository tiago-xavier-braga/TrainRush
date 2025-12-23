using UnityEngine;

namespace XaviGames.SaveSystem
{
    public abstract class ModelType<T> : Model
    {
        [field: SerializeField]
        public T Value { get; private set; }

        [field: SerializeField]
        public T DefaultValue { get; private set; }

        protected void UpdateValue(T value)
        {
            Value = value;
        }

        public override object GetValue()
        {
            return Value;
        }

        public override void Save(IDataStorage storage)
        {
            switch (Value)
            {
                case int value:
                    storage.SetInt(Key, value);
                    break;
                case float value:
                    storage.SetFloat(Key, value);
                    break;
                case string value:
                    storage.SetString(Key, value);
                    break;
            }
        }

        public override void Load(IDataStorage storage)
        {
            if (!storage.HasKey(Key))
            {
                UpdateValue(DefaultValue);
                return;
            }

            if (typeof(T) == typeof(int))
            {
                UpdateValue((T)(object)storage.GetInt(Key));
            }
            else if (typeof(T) == typeof(float))
            {
                UpdateValue((T)(object)storage.GetFloat(Key));
            }
            else if (typeof(T) == typeof(string))
            {
                UpdateValue((T)(object)storage.GetString(Key));
            }
        }
    }
}
