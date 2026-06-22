using System;
using System.Threading;
using SaberCombatMeta.App.Contracts;
using SaberCombatMeta.Meta;
using SaberCombatMeta.UI;
using UnityEngine;
using Zenject;

namespace SaberCombatMeta.Simulation
{
    public class SimulationSession: Session, IDisposable
    {
        [SerializeField]
        private Transform _playerLocation;
        
        [SerializeField]
        private HudScreen _hudScreen;
        
        [SerializeField]
        private CombatResultScreen _combatResultScreen;

        private PlayerManager _playerManager;
        private EnemyManager _enemyManager;
        private CameraController _cameraController;
        private CombatStatisticsRecorder _statisticsRecorder;
        
        private IApplication _application; 
        private MetaController _metaController;
        
        [Inject]
        private void Construct(
            PlayerManager playerManager,
            EnemyManager enemyManager,
            CameraController cameraController,
            CombatStatisticsRecorder statisticsRecorder,
            IApplication application,
            MetaController metaController)
        {
            _playerManager = playerManager;
            _enemyManager = enemyManager;
            _cameraController = cameraController;
            _statisticsRecorder = statisticsRecorder;
            _application = application;
            _metaController = metaController;
        }

        public override async Awaitable InitializeAsync(CancellationToken token)
        {
            _statisticsRecorder.CombatFinished += OnCombatFinished;
            
            var activePlayer = await SpawnPlayerAsync();
            _hudScreen.Show(activePlayer);
            
            async Awaitable<PlayerEntity> SpawnPlayerAsync()
            {
                var player = await _playerManager.SpawnAsync(_playerLocation, token);
                player.CameraController = _cameraController;
                _cameraController.Target = player.transform;
                return player;
            }
        }

        public override void Dispose()
        {
            _statisticsRecorder.CombatFinished -= OnCombatFinished;
            
            _playerManager.Dispose();
            _enemyManager.Dispose();
        }
        
        private void OnCombatFinished(Combat combat)
        {
            if (_playerManager.Player != null) 
                _playerManager.Player.SetEnabledInputs(false);
            
            _metaController.Profile.AddCombat(combat);
            _metaController.SaveAsync(_application.QuitToken).LogExceptionsAndForget();
            
            _hudScreen.Hide();
            _combatResultScreen.Show(combat);
        }
    }
}