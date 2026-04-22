using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Gameplay._Views {
    public class ResultDialog : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private Button _continueButton;

        private bool _lastResultIsWin;
        public event Action<bool> OnContinueClicked;

        private void Awake() {
            _continueButton.onClick.AddListener(() => OnContinueClicked?.Invoke(_lastResultIsWin));
        }

        public void Show(bool isWin) {
            _lastResultIsWin = isWin;
            _titleText.text = isWin ? "LEVEL CLEARED!" : "GAME OVER";
            _titleText.color = isWin ? Color.green : Color.red;

            gameObject.SetActive(true);
        }

        public void Hide() => gameObject.SetActive(false);
    }
}