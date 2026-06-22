using System.Threading;
using UnityEngine;

namespace SaberCombatMeta.Simulation
{
    public interface IAsyncAbility
    {
        protected internal Awaitable UseAsync(object parameters, CancellationToken token);
    }
}