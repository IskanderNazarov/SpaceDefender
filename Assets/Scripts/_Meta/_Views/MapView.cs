using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Meta._Views {
    public class MapView : MonoBehaviour, IMapView {
        [SerializeField] private List<LevelNode> _levelNodes;

        public event Action<int> OnLevelSelected;

        private void Awake() {
            for (var i = 0; i < _levelNodes.Count; i++) {
                var index = i;
                _levelNodes[i].OnClicked += idx => OnLevelSelected?.Invoke(idx);
            }
        }

        public void UpdateMapState(int maxUnlockedLevel) {
            for (var i = 0; i < _levelNodes.Count; i++) {
                var isLocked = i > maxUnlockedLevel;
                var isPassed = i < maxUnlockedLevel;
                var isActive = i == maxUnlockedLevel;
                _levelNodes[i].Setup(i, isLocked, isPassed, isActive);
            }
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}