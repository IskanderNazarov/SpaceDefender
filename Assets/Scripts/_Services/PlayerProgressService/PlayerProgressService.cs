using System.Collections.Generic;
using _Gameplay._Models;
using _Services._DI;
using _Services._SaveService;
using UnityEngine;

namespace _Services.PlayerProgressService {
    public class PlayerProgressService : IInitializable {
        [Inject] private IDataSaver _dataSaver;

        private const string SaveKey = "PlayerProgress";
        private PlayerProgressData _cachedData;
        private Dictionary<int, LevelData> _generatedLevels;

        public int MaxUnlockedLevel => _cachedData.MaxUnlockedLevel;

        public void Initialize() {
            Load();
        }

        public LevelData GetLevelData(int index) {
            _generatedLevels.TryGetValue(index, out var data);
            return data;
        }

        public void SaveLevelData(int index, LevelData data) {
            _generatedLevels[index] = data;
            Save();
        }

        public void SetMaxUnlockedLevel(int index) {
            if (index > _cachedData.MaxUnlockedLevel) {
                _cachedData.MaxUnlockedLevel = index;
                Save();
            }
        }

        private void Save() {
            _cachedData.SetLevelsDictionary(_generatedLevels);
            string json = JsonUtility.ToJson(_cachedData);
            Debug.Log($"[SAVE] json: {json}");
            _dataSaver.SaveDataData(SaveKey, json, null);
        }

        private void Load() {
            _dataSaver.LoadData(SaveKey, (data, success) => {
                if (success && !string.IsNullOrEmpty(data)) {
                    _cachedData = JsonUtility.FromJson<PlayerProgressData>(data);
                    Debug.Log($"[LOAD] data: {data}, _cachedData: {_cachedData == null}");
                    _generatedLevels = _cachedData.GetLevelsDictionary();
                } else {
                    _cachedData = new PlayerProgressData();
                    _generatedLevels = new Dictionary<int, LevelData>();
                }
            });
        }
    }
}