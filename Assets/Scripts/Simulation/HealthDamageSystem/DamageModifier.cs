using UnityEngine;

namespace SaberCombatMeta.Simulation
{
    public abstract class DamageModifier: MonoBehaviour
    {
        public abstract float ModifyDamage(float damage);
    }
}