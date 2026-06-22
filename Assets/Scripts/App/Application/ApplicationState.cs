using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using SaberCombatMeta.App.Contracts;
using SaberCombatMeta.Common;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace SaberCombatMeta.App
{
    [Serializable]
    public abstract class ApplicationState
    {
        [SerializeField]
        private string _sceneName;
        
        protected string SceneName => _sceneName;

        public abstract Awaitable LoadAsync(CancellationToken token);

        public abstract Awaitable UnloadAsync(CancellationToken token);
    }
    
    [Serializable]
    public class ApplicationState<TSession>: ApplicationState
        where TSession : Session
    {
        [MaybeNull]
        private TSession Session { get; set; }

        public override async Awaitable LoadAsync(CancellationToken token)
        {
            Assert.IsNull(Session);
            
            var scene = await SceneManagerExtensions.LoadSceneAsync(SceneName, LoadSceneMode.Single, token);
            Session = scene.FindComponent<TSession>();
            
            await Session.InitializeAsync(token);
        }
        
        public override Awaitable UnloadAsync(CancellationToken token)
        {
            Assert.IsNotNull(Session);
            
            Session.Dispose();
            Session = null;
            
            return SceneManagerExtensions.UnloadSceneAsync(SceneName, token);
        }
    }
}