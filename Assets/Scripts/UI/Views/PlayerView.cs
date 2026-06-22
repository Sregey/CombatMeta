using SaberCombatMeta.Simulation;
using UnityEngine;

namespace SaberCombatMeta.UI
{
    public class PlayerView: MonoBehaviour
    {
        [SerializeField]
        private HealthView _healthView;
        
        [SerializeField]
        private AbilityView _dashAbilityView;
        
        [SerializeField]
        private AbilityView _aoeAbilityView;

        public void Show(PlayerEntity playerEntity)
        {
            _healthView.Show(playerEntity.Health);
            _dashAbilityView.Show(playerEntity.DashAbility);
            _aoeAbilityView.Show(playerEntity.AoeAttackAbility);
        }
        
        public void Hide()
        {
            _healthView.Hide();
            _dashAbilityView.Hide();
            _aoeAbilityView.Hide();
        }
        
    }
}