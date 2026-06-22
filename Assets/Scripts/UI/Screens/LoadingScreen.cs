using UnityEngine;

namespace SaberCombatMeta.UI
{
    public class LoadingScreen: MonoBehaviour
    {
        [SerializeField]
        private Canvas _canvas;
        
        public void Show()
        {
            _canvas.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }
    }
}