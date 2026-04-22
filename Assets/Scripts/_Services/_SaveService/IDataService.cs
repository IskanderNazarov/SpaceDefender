using System;

namespace _Services._SaveService {
    public interface IDataSaver {
        void LoadData(string key, Action<string, bool> onLoaded);
        public void SaveDataData(string key, string data, Action<bool> onSaved);
    }
}