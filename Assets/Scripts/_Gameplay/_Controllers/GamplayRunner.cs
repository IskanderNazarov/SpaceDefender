using _Services._DI;
using UnityEngine;

namespace _Gameplay._Controllers {
    public class GameplayRunner : MonoBehaviour {
        [Inject] private GameplayController _gameplayController;

        private void Start() {
            DIContainer.Inject(this);
        }

        private void Update() {
            // Вызываем Tick только если контроллер проинициализирован
            _gameplayController?.Tick(Time.deltaTime);
        }
    }
}