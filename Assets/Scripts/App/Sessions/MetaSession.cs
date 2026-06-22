using System.Threading;
using System.Threading.Tasks;
using SaberCombatMeta.App.Contracts;
using SaberCombatMeta.UI;
using UnityEngine;

namespace SaberCombatMeta.Meta
{
    public class MetaSession: Session
    {
        [SerializeField]
        private MetaScreen _metaScreen;
        
        public override async Awaitable InitializeAsync(CancellationToken token)
        {
            _metaScreen.Show();
            await Task.CompletedTask;
        }

        public override void Dispose()
        {
            _metaScreen.Hide();
        }
    }
}