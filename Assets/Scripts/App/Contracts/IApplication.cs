using System.Threading;
using UnityEngine;

namespace SaberCombatMeta.App.Contracts
{
    public interface IApplication
    {
        public CancellationToken QuitToken { get; }

        public Awaitable LoadSimulationStateAsync(CancellationToken token);

        public Awaitable LoadMetaStateAsync(CancellationToken token);
    }
}