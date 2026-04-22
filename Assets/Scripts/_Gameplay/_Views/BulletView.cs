using System;
using UnityEngine;

namespace _Gameplay._Views {
    public class BulletView : MonoBehaviour {
        [SerializeField] private float _speed = 10f;
        
        public event Action<BulletView> OnHit;
        public event Action<BulletView> OnOffScreen;

        private Camera _mainCamera;

        public void Setup(Vector2 startPosition) {
            transform.position = startPosition;
            _mainCamera = Camera.main;
        }

        private void Update() {
            transform.Translate(Vector3.up * (_speed * Time.deltaTime));

            if (_mainCamera != null) {
                Vector2 viewportPos = _mainCamera.WorldToViewportPoint(transform.position);
                if (viewportPos.y > 1.1f) {
                    OnOffScreen?.Invoke(this);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Asteroid")) {
                OnHit?.Invoke(this);
            }
        }
    }
}