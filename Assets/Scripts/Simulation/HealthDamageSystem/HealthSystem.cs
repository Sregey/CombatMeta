using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaberCombatMeta.Simulation
{
    public class HealthSystem
    {
        public event Action<Health, float> DamageDealt;
        
        public void TakeDamage(Health health, float damage, List<DamageModifier> damageModifiers)
        {
            damage = ApplyDamageModifiers(damage, damageModifiers);
            TakeDamage(health, damage);
        }

        public void TakeDamage(Health health, float damage)
        {
            damage = ApplyDamageModifiers(damage, health.DamageModifiers);
            damage = Mathf.Min(damage, health.CurrentHealth);

            if (damage > 0)
            {
                health.CurrentHealth -= damage;
                DamageDealt?.Invoke(health, damage);
            }
        }

        private float ApplyDamageModifiers(float damage, List<DamageModifier> damageModifiers)
        {
            foreach (var modifier in damageModifiers)
            {
                damage = modifier.ModifyDamage(damage);
            }
            return damage;
        }
    }
}