using System;
using _Data;
using _Gameplay._Models;
using _Gameplay._Views;
using _Meta._Views._UI;
using _Services._DI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Gameplay._Controllers {
    public class GameplayController {
        public event Action<bool> OnLevelFinished;

        private PlayerController _playerController;
        private AsteroidsController _asteroidsController;
        private PlayerModel _playerModel;
        private int _asteroidsDestroyed = 0;
        private int _hitToWin;
        private bool _isPlaying = false;
        private GameObject _levelRoot;

        [Inject] private GameConfig _gameConfig;
        [Inject] private GameplayUI _gameplayUI;
        [Inject] private ResultDialog _resultDialog;

        public void StartLevel(LevelData levelData) {
            _hitToWin = levelData.AsteroidsHitToWin;
            _asteroidsDestroyed = 0;
            _isPlaying = true;

            _levelRoot = new GameObject("--- GAMEPLAY ROOT ---");

            // 1. Setup Model
            _playerModel = new PlayerModel(3);
            _playerModel.OnHealthChanged += HandleHealthChanged;
            _playerModel.OnDied += HandlePlayerDied;

            // 2. Setup Player View & Controller
            var playerViewInstance = Object.Instantiate(_gameConfig.PlayerViewPrefab, _levelRoot.transform);
            _playerController = new PlayerController(_playerModel, playerViewInstance, _gameConfig.BulletViewPrefab, _levelRoot.transform);

            // 3. Setup Asteroids Controller
            _asteroidsController = new AsteroidsController(levelData, _gameConfig, _levelRoot.transform);
            _asteroidsController.OnAsteroidHitPlayer += () => {
                if (_isPlaying) _playerModel.TakeDamage(1);
            };
            _asteroidsController.OnAsteroidDestroyed += () => {
                if (_isPlaying) HandleAsteroidDestroyed();
            };

            _resultDialog.OnContinueClicked += HandleResultDialogClosed;

            _gameplayUI.Show();
            _gameplayUI.UpdateHealth(_playerModel.CurrentHealth);
            Debug.Log("[Gameplay] Level Started!");
        }

        public void Tick(float deltaTime) {
            if (!_isPlaying) return;

            _playerController?.Tick(deltaTime);
            _asteroidsController?.Tick(deltaTime);
        }

        private void HandleAsteroidDestroyed() {
            _asteroidsDestroyed++;
            _gameplayUI.UpdateScore(_asteroidsDestroyed, _hitToWin);

            if (_asteroidsDestroyed >= _hitToWin) {
                WinGame();
            }
        }

        private void HandleHealthChanged(int currentHealth) {
            _gameplayUI.UpdateHealth(currentHealth);
        }

        private void HandlePlayerDied() {
            _isPlaying = false;
            _resultDialog.Show(isWin: false);
        }

        private void WinGame() {
            _isPlaying = false;
            _resultDialog.Show(isWin: true);
        }

        private void HandleResultDialogClosed(bool isWin) {
            _resultDialog.Hide();
            _gameplayUI.Hide();

            OnLevelFinished?.Invoke(isWin);
        }

        public void Cleanup() {
            if (_levelRoot != null) {
                Object.Destroy(_levelRoot);
            }

            _resultDialog.OnContinueClicked -= HandleResultDialogClosed;

            _playerController = null;
            _asteroidsController = null;
            _playerModel = null;
        }
    }
}