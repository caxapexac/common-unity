using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Data
{
    /// <summary>
    /// Stores it's data in PlayerPrefs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public sealed class Pref<T>
    {
        private readonly string _playerPrefsPath;
        private T _value;
        private T _prevValue;
        public event Action OnChanged = () => { };

        public T Value
        {
            get => _value;
            set
            {
                _prevValue = _value;
                _value = value;
                SaveToPrefs();
                OnChanged.Invoke();
            }
        }

        public T PrevValue
        {
            get => _prevValue;
        }

        public Pref(string playerPrefsPath, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(playerPrefsPath)) throw new Exception("playerPrefsPath is empty");
            _playerPrefsPath = playerPrefsPath;
            _value = defaultValue;
            _prevValue = defaultValue;
            LoadFromPrefs();
        }

        private void LoadFromPrefs()
        {
            if (!PlayerPrefs.HasKey(_playerPrefsPath))
            {
                SaveToPrefs();
                return;
            }

            var stringToDeserialize = PlayerPrefs.GetString(_playerPrefsPath, "");

            var bytes = Convert.FromBase64String(stringToDeserialize);
            var memoryStream = new MemoryStream(bytes);
            var bf = new BinaryFormatter();

            _value = (T)bf.Deserialize(memoryStream);
        }

        private void SaveToPrefs()
        {
            var memoryStream = new MemoryStream();
            var bf = new BinaryFormatter();
            bf.Serialize(memoryStream, _value);
            var stringToSave = Convert.ToBase64String(memoryStream.ToArray());
            PlayerPrefs.SetString(_playerPrefsPath, stringToSave);
        }
    }
}