using System;
using _Gameplay._Models;
using _Gameplay._Views;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

namespace _Gameplay._Controllers {
    public class PlayerController {
        private readonly PlayerModel _model;
        private readonly PlayerView _view;
        private readonly BulletView _bulletPrefab;
        private readonly Camera _camera;
        private readonly Transform _rootTransform;

        private ObjectPool<BulletView> _bulletPool;
        
        private float _fireRate = 0.75f;
        private float _fireTimer;

        // Viewport bounds (0 to 1)
        private readonly float _minX = 0.05f; // 5%
        private readonly float _maxX = 0.95f; // 95%
        private readonly float _minY = 0.1f;  // 10%
        private readonly float _maxY = 0.4f;  // 40%

        public PlayerController(PlayerModel model, PlayerView view, BulletView bulletPrefab, Transform rootTransform) {
            _model = model;
            _view = view;
            _bulletPrefab = bulletPrefab;
            _rootTransform = rootTransform;
            _camera = Camera.main;

            InitializePool();
        }

        private void InitializePool() {
            _bulletPool = new ObjectPool<BulletView>(
                createFunc: () => {
                    var bullet = UnityEngine.Object.Instantiate(_bulletPrefab, _rootTransform);
                    bullet.OnHit += ReturnBulletToPool;
                    bullet.OnOffScreen += ReturnBulletToPool;
                    return bullet;
                },
                actionOnGet: bullet => bullet.gameObject.SetActive(true),
                actionOnRelease: bullet => bullet.gameObject.SetActive(false),
                actionOnDestroy: bullet => UnityEngine.Object.Destroy(bullet.gameObject),
                defaultCapacity: 10,
                maxSize: 30
            );
        }

        public void Tick(float deltaTime) {
            HandleInput();
            HandleShooting(deltaTime);
        }

        private void HandleInput() {
            // Pointer.current автоматически подхватывает и мышь на ПК, и тачскрин на Android
            if (Pointer.current != null && Pointer.current.press.isPressed) {
                // Читаем позицию курсора/пальца
                var screenPos = Pointer.current.position.ReadValue();
        
                Vector2 worldPos = _camera.ScreenToWorldPoint(screenPos);
                Vector2 viewportPos = _camera.WorldToViewportPoint(worldPos);

                // Clamp within requested percentage bounds
                viewportPos.x = Mathf.Clamp(viewportPos.x, _minX, _maxX);
                viewportPos.y = Mathf.Clamp(viewportPos.y, _minY, _maxY);

                Vector2 clampedWorldPos = _camera.ViewportToWorldPoint(viewportPos);
                _view.SetPosition(clampedWorldPos);
            }
        }

        private void HandleShooting(float deltaTime) {
            _fireTimer += deltaTime;
            if (_fireTimer >= _fireRate) {
                _fireTimer = 0f;
                Shoot();
            }
        }

        private void Shoot() {
            var bullet = _bulletPool.Get();
            bullet.Setup(_view.ShootPosition);
        }

        private void ReturnBulletToPool(BulletView bullet) {
            _bulletPool.Release(bullet);
        }
    }
}