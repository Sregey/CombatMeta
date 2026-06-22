using System;
using System.Threading;
using UnityEngine;

namespace SaberCombatMeta.App.Contracts
{
    public abstract class Session: MonoBehaviour, IDisposable
    {
        public abstract Awaitable InitializeAsync(CancellationToken token);
        
        public abstract void Dispose();
    }
}