using SaberCombatMeta.Simulation;
using UnityEngine;
using UnityEngine.UI;

namespace SaberCombatMeta.UI
{
    public class AbilityView: MonoBehaviour
    {
        [SerializeField]
        private Slider _cooldownSlider;
        
        private ICooldownAbility _ability;

        public void Show(ICooldownAbility ability)
        {
            _ability =  ability;
            enabled = true;
        }
        
        public void Hide()
        {
            enabled = false;
        }

        private void Update()
        {
            _cooldownSlider.value = Mathf.InverseLerp(0, _ability.Cooldown.Duration, _ability.Cooldown.RemainingTime);
        }
    }
}