using System;
using _Gameplay._Models;
using _Meta._Views;
using _Services;
using _Services._DI;
using _Services.PlayerProgressService;

namespace _Meta._Controllers {
    public class MapController {
        [Inject] private PlayerProgressService _progressService;
        [Inject] private LevelGenerator _levelGenerator;

        private IMapView _mapView;
        private ILevelInfoDialog _infoDialog;
        private int _selectedLevelIndex;

        public event Action<LevelData> OnStartLevelRequested;

        public void Initialize(IMapView mapView, ILevelInfoDialog infoDialog) {
            _mapView = mapView;
            _infoDialog = infoDialog;

            // Подписываемся на события только один раз при старте
            _mapView.OnLevelSelected += HandleLevelSelected;
            _infoDialog.OnPlayRequested += HandlePlayRequested;
            _infoDialog.OnCloseRequested += () => _infoDialog.Hide();
        }

        // Вызываем каждый раз, когда нужно отобразить карту (при старте и после завершения уровня)
        public void ShowMap() {
            // Запрашиваем свежий прогресс (вдруг мы только что прошли новый уровень)
            _mapView.UpdateMapState(_progressService.MaxUnlockedLevel);
            _mapView.Show();
        }

        private void HandleLevelSelected(int index) {
            _selectedLevelIndex = index;
            int maxUnlocked = _progressService.MaxUnlockedLevel;

            if (index > maxUnlocked) {
                _infoDialog.Show(null, true);
                return;
            }

            LevelData data;
            if (index < maxUnlocked) {
                data = _levelGenerator.GenerateLevelData(index);
                _progressService.SaveLevelData(index, data);
            } else {
                data = _progressService.GetLevelData(index);
                if (data == null) {
                    data = _levelGenerator.GenerateLevelData(index);
                    _progressService.SaveLevelData(index, data);
                }
            }

            _infoDialog.Show(data, false);
        }

        private void HandlePlayRequested() {
            LevelData data = _progressService.GetLevelData(_selectedLevelIndex);
            if (data != null) {
                _mapView.Hide();
                _infoDialog.Hide();
                OnStartLevelRequested?.Invoke(data);
            }
        }
    }
}