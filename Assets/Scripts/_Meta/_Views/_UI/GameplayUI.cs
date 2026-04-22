using TMPro;
using UnityEngine;

namespace _Meta._Views._UI {
    public class GameplayUI : MonoBehaviour {
        [SerializeField] private GameObject[] _heartIcons;
        [SerializeField] private TextMeshProUGUI _scoreText;

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        public void UpdateHealth(int currentHealth) {
            for (int i = 0; i < _heartIcons.Length; i++) {
                _heartIcons[i].SetActive(i < currentHealth);
            }
        }

        public void UpdateScore(int current, int target) {
            _scoreText.text = $"Asteroid hit:\n{current}/{target}";
        }
    }
}