using System;
using _Gameplay._Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Services {
    public class LevelGenerator {
        public LevelData GenerateLevelData(int levelIndex) {
            var data = new LevelData();

            // 1. Randomize Asteroid Types (1 to 3 types per level)
            var typesCount = Random.Range(1, 4);
            data.AsteroidTypes = new AsteroidType[typesCount];
            var allAsteroidTypes = Enum.GetValues(typeof(AsteroidType));

            for (var i = 1; i <= typesCount; i++) {
                //data.AsteroidTypes[i] = (AsteroidType)allAsteroidTypes.GetValue(Random.Range(0, allAsteroidTypes.Length));
                data.AsteroidTypes[i - 1] = (AsteroidType)allAsteroidTypes.GetValue(i);
            }

            // 2. Randomize Size Factor
            var randomSize = Random.Range(0.8f, 1.2f);
            data.AsteroidsSizeFactor = (float)Math.Round(randomSize, 2);

            // 3. Randomize Spawn Rate (Increase difficulty based on levelIndex)
            var baseSpawnRate = Random.Range(1.5f, 4.0f);
            var difficultyMultiplier = levelIndex * 0.5f;
            data.AsteroidsSpawnRate = (float)Math.Round(baseSpawnRate + difficultyMultiplier, 2);

            //4. Randomize asteroids win count
            var t = levelIndex / 20;
            var count = Mathf.Lerp(30, 100, t);
            data.AsteroidsHitToWin = (int)count;

            return data;
        }
    }
}