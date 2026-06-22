using System.Threading;
using SaberCombatMeta.App;
using UnityEngine;

namespace SaberCombatMeta.Meta
{
    public class MetaController
    {
        private readonly ISaveController _saveController;
        private Profile _profile;
        
        public Profile Profile => _profile;
        
        public MetaController(ISaveController saveController)
        {
            _saveController = saveController;
        }
        
        public async Awaitable InitializeAsync(CancellationToken token)
        {
            _profile = await _saveController.LoadAsync(token);
        }

        public Awaitable SaveAsync(CancellationToken token)
        {
            return _saveController.SaveAsync(_profile, token);
        }
    }
}