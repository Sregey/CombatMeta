using SaberCombatMeta.App.Contracts;
using SaberCombatMeta.Meta;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SaberCombatMeta.UI
{
    public class CombatResultScreen: MonoBehaviour
    {
        [SerializeField]
        private Canvas _canvas;
        
        [SerializeField]
        private Text _scoreText;

        [SerializeField]
        private CombatView _combatView;
        
        private IApplication _application;
        private MetaController _metaController;
        
        [Inject]
        private void Construct(IApplication application, MetaController metaController)
        {
            _application = application;
            _metaController = metaController;
        }

        public void Show(Combat combat)
        {
            _canvas.enabled = true;
            Cursor.lockState = CursorLockMode.Confined;

            _scoreText.text = _metaController.Profile.LastCombatScore.ToString("N0");
            _combatView.Show(combat);
        }

        public void Hide()
        {
            _canvas.enabled = false;
            
            _combatView.Hide();
        }
        
        public void OpenMeta()
        {
            Hide();
            _application.LoadMetaStateAsync(_application.QuitToken).LogExceptionsAndForget();
        }
    }
}