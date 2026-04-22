using System;
using System.Collections.Generic;
using _Gameplay._Models;

[Serializable]
public class PlayerProgressData {
    public int MaxUnlockedLevel = 0;
    
    // Serialization for Dictionary
    public List<int> LevelIndices = new List<int>();
    public List<LevelData> LevelValues = new List<LevelData>();
    
    // You can add more complex structures here later, e.g.,
    // public PlayerInventoryData Inventory;

    public Dictionary<int, LevelData> GetLevelsDictionary() {
        var dict = new Dictionary<int, LevelData>();
        for (int i = 0; i < LevelIndices.Count; i++) {
            dict[LevelIndices[i]] = LevelValues[i];
        }
        return dict;
    }

    public void SetLevelsDictionary(Dictionary<int, LevelData> dict) {
        LevelIndices.Clear();
        LevelValues.Clear();
        foreach (var kvp in dict) {
            LevelIndices.Add(kvp.Key);
            LevelValues.Add(kvp.Value);
        }
    }
}