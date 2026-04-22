using System;
using _Gameplay._Models;
using UnityEngine;

namespace _Gameplay._Views {
    public class AsteroidView : MonoBehaviour {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _speed = 3f;

        public AsteroidType Type { get; private set; }
        public event Action<AsteroidView> OnHitPlayer;
        public event Action<AsteroidView> OnHitBullet;
        public event Action<AsteroidView> OnOffScreen;

        private Camera _camera;

        public void Setup(AsteroidType type, float sizeFactor, Vector2 startPosition, Color color) {
            _camera = Camera.main;
            Type = type;
            transform.position = startPosition;
            var scale = Vector3.one * sizeFactor;
            scale.z = 1;
            transform.localScale = scale;

            if (_spriteRenderer != null) {
                _spriteRenderer.color = color;
            }
        }

        private void Update() {
            transform.Translate(Vector3.down * (_speed * Time.deltaTime));

            Vector2 viewportPos = _camera.WorldToViewportPoint(transform.position);
            if (viewportPos.y < -0.1f) {
                OnOffScreen?.Invoke(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                OnHitPlayer?.Invoke(this);
            } else if (other.CompareTag("Bullet")) {
                OnHitBullet?.Invoke(this);
            }
        }
    }
}