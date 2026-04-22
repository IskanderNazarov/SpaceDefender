using _Meta._Views;
using _Services._DI;
using UnityEngine;

namespace _Core {
    public class Bootstrapper : MonoBehaviour {
        [SerializeField] private DIInstaller _diInstaller;
        [SerializeField] private MapView _mapView;
        [SerializeField] private LevelInfoDialog _infoDialog;

        [Inject] private GameManager _gameManager;

        private void Start() {
            _diInstaller.Install(this);
            _gameManager.InitializeGameFlow(_mapView, _infoDialog);
        }
    }
}