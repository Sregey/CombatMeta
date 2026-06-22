using System.Collections.Generic;
using System.Linq;
using SaberCombatMeta.App.Contracts;
using SaberCombatMeta.Meta;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SaberCombatMeta.UI
{
    public class MetaScreen: MonoBehaviour
    {
        [SerializeField]
        private Canvas _canvas;
        
        [SerializeField]
        private Text _scoreText;

        [SerializeField]
        private RectTransform _combatListView;
        
        [SerializeField]
        private CombatView _combatViewPrefab;
        
        private readonly List<CombatView> _combatViews = new();
        
        private IApplication _application;
        private MetaController _metaController;
        
        [Inject]
        private void Construct(IApplication application, MetaController metaController)
        {
            _application = application;
            _metaController = metaController;
        }

        public void Show()
        {
            _canvas.enabled = true;
            Cursor.lockState = CursorLockMode.Confined;
            
            _scoreText.text = _metaController.Profile.Score.ToString("N0");
             foreach (var combat in _metaController.Profile.Combats.Reverse())
             {
                 var combatView = Instantiate(_combatViewPrefab, _combatListView);
                 combatView.Show(combat);
                 _combatViews.Add(combatView);
             }
        }

        public void Hide()
        {
            _canvas.enabled = false;

            foreach (var combatView in _combatViews)
            {
                combatView.Hide();
                Destroy(combatView.gameObject);
            }
        }

        public void StartCombat()
        {
            _application.LoadSimulationStateAsync(_application.QuitToken).LogExceptionsAndForget();
        }
    }
}