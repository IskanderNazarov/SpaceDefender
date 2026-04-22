using UnityEngine;

namespace _Gameplay._Views {
    public class PlayerView : MonoBehaviour {
        [SerializeField] private Transform _shootPoint;

        public Vector2 ShootPosition => _shootPoint.position;

        public void SetPosition(Vector2 position) {
            transform.position = position;
        }
    }
}