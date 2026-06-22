using System.Threading;
using SaberCombatMeta.Meta;
using UnityEngine;

namespace SaberCombatMeta.Meta
{
    public interface ISaveController
    {
        Awaitable SaveAsync(Profile profile, CancellationToken token);
        Awaitable<Profile> LoadAsync(CancellationToken token);
    }
}