using UnityEngine;

namespace SaberCombatMeta.Simulation
{
    public class ArmorDamageModifier: DamageModifier
    {
        [SerializeField, Min(0)]
        private float _armor;
        
        public override float ModifyDamage(float damage)
        {
            return Mathf.Max(damage - _armor, 0);
        }
    }
}