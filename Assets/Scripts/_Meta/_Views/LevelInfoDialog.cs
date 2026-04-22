using System;
using _Data;
using _Gameplay._Models;
using _Services._DI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Meta._Views {
    public class LevelInfoDialog : MonoBehaviour, ILevelInfoDialog {
        [Header("UI Elements")] [SerializeField]
        private TextMeshProUGUI _asteroidTypesText;

        [SerializeField] private Image[] _asteroidTypeImages;
        [SerializeField] private TextMeshProUGUI _sizeFactorText;
        [SerializeField] private TextMeshProUGUI _spawnRateText;
        [SerializeField] private TextMeshProUGUI _winCountText;

        [Header("Controls")] [SerializeField] private Button _playButton;
        [SerializeField] private Button _closeButton;
        
        public event Action OnPlayRequested;
        public event Action OnCloseRequested;
        
        [Inject] private GameConfig _gameConfig;

        private void Awake() {
            _playButton.onClick.AddListener(() => OnPlayRequested?.Invoke());
            _closeButton.onClick.AddListener(() => OnCloseRequested?.Invoke());
        }

        public void Show(LevelData data, bool isLocked) {
            gameObject.SetActive(true);

            if (isLocked) {
                SetUnknownState();
                _playButton.gameObject.SetActive(false);
            } else {
                SetLevelDataState(data);
                _playButton.gameObject.SetActive(true);
            }
        }

        public void Hide() => gameObject.SetActive(false);

        private void SetUnknownState() {
            _asteroidTypesText.text = "Types: ???";
            _sizeFactorText.text = "Size: ???";
            _spawnRateText.text = "Speed: ???";
            _winCountText.text = "Count: ???";

            foreach (var img in _asteroidTypeImages) {
                img.gameObject.SetActive(false);
            }
        }

        private void SetLevelDataState(LevelData data) {
            if (data == null) return;

            _asteroidTypesText.text = "Types:";
            _sizeFactorText.text = $"Size Factor: x{data.AsteroidsSizeFactor:F2}";
            _spawnRateText.text = $"Spawn Rate: {data.AsteroidsSpawnRate:F2}/sec";
            _winCountText.text = $"Hits to win: {data.AsteroidsHitToWin}";

            for (int i = 0; i < _asteroidTypeImages.Length; i++) {
                if (i < data.AsteroidTypes.Length) {
                    _asteroidTypeImages[i].gameObject.SetActive(true);
                    _asteroidTypeImages[i].color = GetColorForAsteroidType(data.AsteroidTypes[i]);
                } else {
                    _asteroidTypeImages[i].gameObject.SetActive(false);
                }
            }
        }

        private Color GetColorForAsteroidType(AsteroidType type) {
            var color = _gameConfig.GetAsteroidInfo(type).AsteroidColor;
            return color;
        }
    }
}