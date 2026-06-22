using SaberCombatMeta.Simulation;
using UnityEngine;
using UnityEngine.UI;

namespace SaberCombatMeta.UI
{
    public class HealthView: MonoBehaviour
    {
        [SerializeField]
        private Slider _healthSlider;

        private Health _health;

        public void Show(Health health)
        {
            _health = health;
            _health.HealthChanged += OnHealthChanged;
            OnHealthChanged(_health);
        }
        
        public void Hide()
        {
            _health.HealthChanged -= OnHealthChanged;
            _health = null;
        }

        private void OnHealthChanged(Health health)
        {
            _healthSlider.value = Mathf.InverseLerp(0, health.MaxHealth, health.CurrentHealth);
        }
    }
}