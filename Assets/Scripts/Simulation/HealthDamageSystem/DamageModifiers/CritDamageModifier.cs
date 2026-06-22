using UnityEngine;

namespace SaberCombatMeta.Simulation
{
    public class CritDamageModifier: DamageModifier
    {
        [SerializeField, Range(0, 1)]
        private float _critChance;
        
        [SerializeField, Min(1)]
        private float _critMultiplier;

        public override float ModifyDamage(float damage)
        {
            if (Random.value <= _critChance)
            {
                damage *= _critMultiplier;
            }
            return damage;
        }
    }
}