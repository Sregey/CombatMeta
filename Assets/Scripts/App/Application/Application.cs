using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using SaberCombatMeta.App.Contracts;
using SaberCombatMeta.Meta;
using SaberCombatMeta.Simulation;
using SaberCombatMeta.UI;
using UnityEngine;
using Zenject;

namespace SaberCombatMeta.App
{
    public class Application: MonoBehaviour, IApplication, IAsyncDisposable
    {
        [SerializeField]
        private ApplicationState<SimulationSession> _simulationState;
        
        [SerializeField]
        private ApplicationState<MetaSession> _metaState;
        
        [SerializeField]
        private LoadingApplicationState _loadingState;
        
        [SerializeField]
        private LoadingScreen _loadingScreen;
        
        [MaybeNull]
        private ApplicationState _activeState;
        
        private MetaController _metaController;
        
        public CancellationToken QuitToken => destroyCancellationToken;
        
        [Inject]
        private void Construct(MetaController metaController)
        {
            _metaController = metaController;
        }

        public async Awaitable InitializeAsync(CancellationToken token)
        {
            await _metaController.InitializeAsync(token);
        }
        
        public async ValueTask DisposeAsync()
        {
            await UnloadActiveStateAsync(CancellationToken.None);
        }

        private void OnApplicationQuit()
        {
            DisposeAsync().GetAwaiter().GetResult();
        }

        public async Awaitable LoadSimulationStateAsync(CancellationToken token)
        {
            await LoadStateAsync(_simulationState, token);
        }
        
        public async Awaitable LoadMetaStateAsync(CancellationToken token)
        {
            await LoadStateAsync(_metaState, token);
        }
        
        private async Awaitable LoadStateAsync(ApplicationState state, CancellationToken token)
        {
            _loadingScreen.Show();
            await _loadingState.LoadAsync(token);
            
            await UnloadActiveStateAsync(token);
            await state.LoadAsync(token);
            _activeState = state;
            
            await _loadingState.UnloadAsync(token);
            _loadingScreen.Hide();
        }

        private async Awaitable UnloadActiveStateAsync(CancellationToken token)
        {
            if (_activeState != null)
            {
                await _activeState.UnloadAsync(token);
                _activeState = null;
            }
        }
    }
}