using System;

namespace _Gameplay._Models {
    [Serializable]
    public class LevelData {
        public AsteroidType[] AsteroidTypes;
        public float AsteroidsSizeFactor = 1; // multiply asteroid transform by this factor
        public float AsteroidsSpawnRate = 5; //asteroid per seconds
        public int AsteroidsHitToWin = 10; //asteroid per seconds
    }

    public enum AsteroidType {
        None = 0, Red = 1, Green = 2, Blue = 3
    }
}