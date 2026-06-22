using SaberCombatMeta.Simulation;
using UnityEngine;
using Zenject;

namespace SaberCombatMeta.App
{
    public class SimulationInstaller: MonoInstaller
    {
        [SerializeField]
        private PlayerManager _playerManager;
        
        [SerializeField]
        private EnemyManager _enemyManager;
        
        [SerializeField]
        private CameraController _cameraController;
        
        [SerializeField]
        private CombatStatisticsRecorder _combatStatisticsRecorder;
        
        public override void InstallBindings()
        {
            Container.Bind<HealthSystem>().AsSingle().NonLazy();
            
            Container.Bind<PlayerManager>().FromInstance(_playerManager).AsSingle();
            Container.Bind<EnemyManager>().FromInstance(_enemyManager).AsSingle();
            
            Container.Bind<CameraController>().FromInstance(_cameraController).AsSingle();
            Container.Bind<CombatStatisticsRecorder>().FromInstance(_combatStatisticsRecorder).AsSingle();
        }
    }
}