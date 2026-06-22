using System;
using SaberCombatMeta.Meta;
using UnityEngine;
using UnityEngine.UI;

namespace SaberCombatMeta.UI
{
    public class CombatView: MonoBehaviour
    {
        [SerializeField]
        private Text _victoryDefeatText;
        
        [SerializeField]
        private Text _durationText;
        
        [SerializeField]
        private Text _damageDealtText;
        
        [SerializeField]
        private Text _damageTakenText;
        
        [SerializeField]
        private Text _enemiesKilledText;
        
        public void Show(Combat combat)
        {
            _victoryDefeatText.text = combat.IsVictory ? "Victory" : "Defeat";
            _durationText.text = $"{combat.Duration:N0} seconds";
            _damageDealtText.text = combat.DamageDealt.ToString("N0");
            _damageTakenText.text = combat.DamageTaken.ToString("N0");
            _enemiesKilledText.text = combat.EnemiesKilled.ToString("N0");
        }
        
        public void Hide()
        {
        }
    }
}