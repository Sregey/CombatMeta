using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace SaberCombatMeta.Common
{
    public static class SceneManagerExtensions
    {
        public static async Awaitable<Scene> LoadSceneAsync(string sceneName, LoadSceneMode mode, CancellationToken token)
        {
            var loadOperation = SceneManager.LoadSceneAsync(sceneName, mode);
            Assert.IsNotNull(loadOperation);
            await Awaitable.FromAsyncOperation(loadOperation, token);
            return SceneManager.GetSceneByName(sceneName);
        }
        
        public static async Awaitable UnloadSceneAsync(string sceneName, CancellationToken token)
        {
            var unloadOperation = SceneManager.UnloadSceneAsync(sceneName);
            if (unloadOperation != null)
            {
                await Awaitable.FromAsyncOperation(unloadOperation, token);
            }
        }
    }
}