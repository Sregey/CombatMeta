using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SaberCombatMeta.Simulation
{
    public class Health: MonoBehaviour
    {
        [SerializeField]
        private MonoBehaviour _owner;
        
        [SerializeField]
        private float _maxHealth = 100;
        
        [SerializeField]
        private List<DamageModifier> _damageModifiers;
        
        private HealthSystem _healthSystem;
        
        private float _currentHealth;
        
        public float CurrentHealth
        {
            get => _currentHealth;
            internal set
            {
                if (!Mathf.Approximately(_currentHealth, value))
                {
                    _currentHealth = value;
                    HealthChanged?.Invoke(this);
                }
            }
        }
        
        public MonoBehaviour Owner => _owner;
        public List<DamageModifier> DamageModifiers => _damageModifiers;

        public float MaxHealth => _maxHealth;
        public bool IsDead => _currentHealth <= 0;
        
        public event Action<Health> HealthChanged;

        [Inject]
        private void Construct(HealthSystem healthSystem)
        {
            _healthSystem = healthSystem;
        }

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(float damage)
        {
            _healthSystem.TakeDamage(this, damage);
        }
        
        public void TakeDamage(float damage, List<DamageModifier> modifiers)
        {
            _healthSystem.TakeDamage(this, damage, modifiers);
        }
    }
}