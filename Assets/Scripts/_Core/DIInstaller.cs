using _Data;
using _Gameplay._Controllers;
using _Gameplay._Views;
using _Meta._Controllers;
using _Meta._Views;
using _Meta._Views._UI;
using _Services;
using _Services._DI;
using _Services._SaveService;
using _Services.PlayerProgressService;
using UnityEngine;

namespace _Core {
    public class DIInstaller : MonoBehaviour {
        [SerializeField] private GameConfig _gameConfig;

        [Space(20)] [SerializeField] private LevelInfoDialog _levelInfoDialog;
        [SerializeField] private GameplayUI _gameplayUI;
        [SerializeField] private ResultDialog _resultDialog;

        private PlayerProgressService _playerProgressService;
        private MapController _mapController;
        private GameplayController _gameplayController;
        private LevelGenerator _levelGenerator;
        private GameManager _gameManager;
        private GameplayRunner _gameplayRunner;

        public void Install(Bootstrapper bootstrapper) {
            _gameManager = new GameManager();
            _playerProgressService = new PlayerProgressService();
            _mapController = new MapController();
            _gameplayController = new GameplayController();
            _levelGenerator = new LevelGenerator();
            _gameplayRunner = CreateGameplayRunner();

            InstallDI();
            InjectDependencies(bootstrapper);

            DIContainer.Initialize();
        }

        private void InstallDI() {
            DIContainer.Install();
            DIContainer.Register<IDataSaver>(new DataSaver_Editor());
            DIContainer.Register(_playerProgressService);
            DIContainer.Register(_levelGenerator);
            DIContainer.Register(_mapController);
            DIContainer.Register(_gameplayController);
            DIContainer.Register(_gameConfig);
            DIContainer.Register(_gameManager);
            DIContainer.Register(_gameplayUI);
            DIContainer.Register(_resultDialog);
        }

        private void InjectDependencies(Bootstrapper bootstrapper) {
            DIContainer.Inject(bootstrapper);
            DIContainer.Inject(_gameManager);
            DIContainer.Inject(_playerProgressService);
            DIContainer.Inject(_mapController);
            DIContainer.Inject(_gameplayController);
            DIContainer.Inject(_gameplayRunner);
            DIContainer.Inject(_levelInfoDialog);
        }

        private GameplayRunner CreateGameplayRunner() {
            var runner = new GameObject("GameplayRunner").AddComponent<GameplayRunner>();
            return runner;
        }
    }
}