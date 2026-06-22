using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SaberCombatMeta.Simulation
{
    public class AbilityManager : MonoBehaviour
    {
        [SerializeField]
        private List<Ability> _abilities;
        
        private readonly HashSet<AbilityTag> _activeTags = new();

        private void Awake()
        {
            foreach (var ability in _abilities)
            {
                ability.Initialize(this);
            }
        }

        private void OnDestroy()
        {
            foreach (var ability in _abilities)
            {
                ability.Dispose();
            }
        }

        public T Get<T>() where T: Ability
        {
            return (T)_abilities.Find(a => a is T);
        }

        public bool CanUse(Ability ability)
        {
            foreach (var blockedByTag in ability.BlockedByTags)
            {
                if (_activeTags.Contains(blockedByTag))
                    return false;
            }

            return true;
        }

        public void AddTags(List<AbilityTag> abilityTags)
        {
            foreach (var abilityTag in abilityTags)
            {
                _activeTags.Add(abilityTag);
            }
        }

        public void RemoveTags(List<AbilityTag> abilityTags)
        {
            foreach (var abilityTag in abilityTags)
            {
                _activeTags.Remove(abilityTag);
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Collect Abilities")]
        private void CollectAbilities()
        {
            _abilities = GetComponents<Ability>().ToList();
            UnityEditor.EditorUtility.SetDirty(this);

        }
#endif
    }
}