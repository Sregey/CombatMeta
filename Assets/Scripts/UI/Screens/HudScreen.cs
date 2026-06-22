using SaberCombatMeta.Simulation;
using UnityEngine;

namespace SaberCombatMeta.UI
{
    public class HudScreen: MonoBehaviour
    {
        [SerializeField]
        private Canvas _canvas;
        
        [SerializeField]
        private PlayerView _playerView;
        
        public void Show(PlayerEntity playerEntity)
        {
            _canvas.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            
            _playerView.Show(playerEntity);
        }

        public void Hide()
        {
            _canvas.enabled = false;
            _playerView.Hide();
        }
    }
}