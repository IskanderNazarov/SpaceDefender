using System;
using _Data;
using _Gameplay._Models;
using _Gameplay._Views;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace _Gameplay._Controllers {
    public class AsteroidsController {
        private readonly LevelData _levelData;
        private readonly GameConfig _gameConfig;
        private readonly AsteroidView _asteroidPrefab;
        private ObjectPool<AsteroidView> _asteroidPool;
        private readonly Transform _rootTransform;

        private float _spawnTimer;
        private float _timeBetweenSpawns;
        private Camera _camera;

        public event Action OnAsteroidDestroyed;
        public event Action OnAsteroidHitPlayer;

        public AsteroidsController(LevelData levelData, GameConfig gameConfig, Transform rootTransform) {
            _levelData = levelData;
            _gameConfig = gameConfig;
            _rootTransform = rootTransform;
            _asteroidPrefab = _gameConfig.AsteroidViewPrefab;
            _camera = Camera.main;

            // SpawnRate is "asteroids per second"
            _timeBetweenSpawns = 1f / _levelData.AsteroidsSpawnRate;

            InitializePool();
        }

        private void InitializePool() {
            _asteroidPool = new ObjectPool<AsteroidView>(
                createFunc: () => {
                    var asteroid = UnityEngine.Object.Instantiate(_asteroidPrefab,  _rootTransform);
                    asteroid.OnHitPlayer += HandleHitPlayer;
                    asteroid.OnHitBullet += HandleHitBullet;
                    asteroid.OnOffScreen += ReturnAsteroidToPool;
                    return asteroid;
                },
                actionOnGet: ast => ast.gameObject.SetActive(true),
                actionOnRelease: ast => ast.gameObject.SetActive(false),
                defaultCapacity: 20,
                maxSize: 50
            );
        }

        public void Tick(float deltaTime) {
            _spawnTimer += deltaTime;
            if (_spawnTimer >= _timeBetweenSpawns) {
                _spawnTimer = 0f;
                SpawnAsteroid();
            }
        }

        private void SpawnAsteroid() {
            var asteroid = _asteroidPool.Get();

            var randomType = _levelData.AsteroidTypes[Random.Range(0, _levelData.AsteroidTypes.Length)];

            // Random X position at the top of the screen (viewport y = 1.1)
            var randomViewportX = Random.Range(0.1f, 0.9f);
            Vector2 spawnWorldPos = _camera.ViewportToWorldPoint(new Vector2(randomViewportX, 1.1f));

            var color = _gameConfig.GetAsteroidInfo(randomType).AsteroidColor;
            asteroid.Setup(randomType, _levelData.AsteroidsSizeFactor, spawnWorldPos, color);
        }

        private void HandleHitPlayer(AsteroidView asteroid) {
            ReturnAsteroidToPool(asteroid);
            OnAsteroidHitPlayer?.Invoke();
        }

        private void HandleHitBullet(AsteroidView asteroid) {
            ReturnAsteroidToPool(asteroid);
            OnAsteroidDestroyed?.Invoke();
        }

        private void ReturnAsteroidToPool(AsteroidView asteroid) {
            _asteroidPool.Release(asteroid);
        }
        
    }
}