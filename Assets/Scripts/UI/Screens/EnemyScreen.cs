using SaberCombatMeta.Simulation;
using UnityEngine;

namespace SaberCombatMeta.UI
{
    public class EnemyScreen: MonoBehaviour
    {
        [SerializeField]
        private EnemyEntity _enemyEntity;
        
        [SerializeField]
        private HealthView _healthView;

        private void Start()
        {
            _healthView.Show(_enemyEntity.Health);
        }

        private void OnDestroy()
        {
            _healthView.Hide();
        }
    }
}