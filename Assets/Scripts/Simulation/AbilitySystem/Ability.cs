using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace SaberCombatMeta.Simulation
{
    public abstract class Ability : MonoBehaviour, IDisposable
    {
        [SerializeField]
        private List<AbilityTag> _tags;
        
        [SerializeField]
        private List<AbilityTag> _blockedByTags;

        protected AbilityManager Manager { get; private set; }
        public bool IsUsing { get; private set; }
        
        public List<AbilityTag> Tags => _tags;
        public List<AbilityTag> BlockedByTags => _blockedByTags;
        
        public virtual void Initialize(AbilityManager manager)
        {
            Manager = manager;
        }

        public virtual void Dispose()
        {
        }

        protected virtual bool CanUse()
        {
            return !IsUsing && Manager.CanUse(this);
        }
        
        protected virtual bool TryBeginUse()
        {
            if (CanUse())
            {
                IsUsing = true;
                Manager.AddTags(Tags);
                return true;
            }

            return false;
        }
        
        protected virtual void EndUse()
        {
            Manager.RemoveTags(Tags);
            IsUsing = false;
        }
        
        public async Awaitable TryUseAsync(object parameters = null, CancellationToken token = default)
        {
            if (!TryBeginUse())
            {
                return;
            }

            if (this is IAsyncAbility asyncAbility)
            {
                try
                {
                    await asyncAbility.UseAsync(parameters, token);
                }
                catch (OperationCanceledException)
                {
                }
                finally
                {
                    EndUse();
                }
            }
            else if (this is ISyncAbility syncAbility)
            {
                syncAbility.Use(parameters);
                EndUse();
            }
            else
            {
                Debug.LogWarning(
                    $"Ability \"{GetType().FullName}\" doesn't implement \"{nameof(IAsyncAbility)}\" or \"{nameof(ISyncAbility)}\" interface.");
                EndUse();
            }
        }
    }
}