using System;
using System.Threading;
using System.Threading.Tasks;
using SaberCombatMeta.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaberCombatMeta.App
{
    [Serializable]
    public class LoadingApplicationState: ApplicationState
    {
        public override async Awaitable LoadAsync(CancellationToken token)
        {
            await SceneManagerExtensions.LoadSceneAsync(SceneName, LoadSceneMode.Additive, token);
        }

        public override async Awaitable UnloadAsync(CancellationToken token)
        {
            await Task.CompletedTask;
        }
    }
}