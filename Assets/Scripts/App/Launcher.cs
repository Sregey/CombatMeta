using UnityEngine;
using Zenject;

namespace SaberCombatMeta.App
{
    public class Launcher: MonoBehaviour
    {
        private void Awake()
        {
            LaunchApplicationAsync().LogExceptionsAndForget();
        }

        private static async Awaitable LaunchApplicationAsync()
        {
            var projectContext = ProjectContext.Instance;
            projectContext.EnsureIsInitialized();
            var application = projectContext.Container.Resolve<Application>();
            
            await application.InitializeAsync(application.QuitToken);
            await application.LoadSimulationStateAsync(application.QuitToken);
        }
    }
}