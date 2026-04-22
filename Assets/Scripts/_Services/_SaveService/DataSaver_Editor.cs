using System;
using UnityEngine;

namespace _Services._SaveService {
    public class DataSaver_Editor : IDataSaver {
        public void LoadData(string key, Action<string, bool> onLoaded) {
            onLoaded?.Invoke(PlayerPrefs.GetString(key), true);
        }

        public void SaveDataData(string key, string data, Action<bool> onSaved) {
            PlayerPrefs.SetString(key, data);
        }
    }
}