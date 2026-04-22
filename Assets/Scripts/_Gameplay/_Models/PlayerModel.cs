using System;

namespace _Gameplay._Models {
    public class PlayerModel {
        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }

        public event Action<int> OnHealthChanged;
        public event Action OnDied;

        public PlayerModel(int maxHealth = 3) {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(int damage) {
            if (CurrentHealth <= 0) return;

            CurrentHealth -= damage;
            OnHealthChanged?.Invoke(CurrentHealth);

            if (CurrentHealth <= 0) {
                OnDied?.Invoke();
            }
        }
    }
}