using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Meta._Views {
    public class LevelNode : MonoBehaviour {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _levelText;
        
        [Header("Visual States")]
        [SerializeField] private GameObject _lockedState;
        [SerializeField] private GameObject _activeState;
        [SerializeField] private GameObject _passedState;

        private int _levelIndex;
        public event Action<int> OnClicked;

        private void Awake() {
            _button.onClick.AddListener(() => OnClicked?.Invoke(_levelIndex));
        }

        public void Setup(int index, bool isLocked, bool isPassed, bool isActive) {
            _levelIndex = index;
            _levelText.text = (index + 1).ToString();
            
            _button.interactable = !isLocked;
            
            _lockedState.SetActive(isLocked);
            _activeState.SetActive(isActive);
            _passedState.SetActive(isPassed);
        }
    }
}