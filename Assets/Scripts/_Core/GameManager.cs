using _Data;
using _Gameplay._Controllers;
using _Gameplay._Models;
using _Meta._Controllers;
using _Meta._Views;
using _Services._DI;
using _Services._SaveService;
using _Services.PlayerProgressService;
using UnityEngine;

namespace _Core {
    public class GameManager {
        [Inject] private IDataSaver _dataSaver;
        [Inject] private MapController _mapController;
        [Inject] private GameplayController _gameplayController;
        [Inject] private PlayerProgressService _progressService;
        [Inject] private GameConfig _gameConfig;

        private int _currentPlayingLevelIndex;

        public void InitializeGameFlow(IMapView mapView, ILevelInfoDialog infoDialog) {
            _mapController.Initialize(mapView, infoDialog);
            _mapController.OnStartLevelRequested += OnStartLevelRequested;
            _mapController.ShowMap();

            _gameplayController.OnLevelFinished += OnLevelFinished;
        }

        private void OnStartLevelRequested(int levelIndex, LevelData levelData) {
            _currentPlayingLevelIndex = levelIndex;
            _gameplayController.StartLevel(levelData);
        }

        private void OnLevelFinished(bool isWin) {
            if (isWin) {
                if (_currentPlayingLevelIndex == _progressService.MaxUnlockedLevel) {
                    _progressService.SetMaxUnlockedLevel(_currentPlayingLevelIndex + 1);
                }
            }

            _gameplayController.Cleanup();
            _mapController.ShowMap();
        }
    }
}