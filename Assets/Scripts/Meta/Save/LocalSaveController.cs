using System.IO;
using System.Threading;
using UnityEngine;

namespace SaberCombatMeta.Meta
{
    public class LocalSaveController: ISaveController
    {
        private readonly string _savePath = Path.Combine(Application.persistentDataPath, "Save.json");

        public async Awaitable SaveAsync(Profile profile, CancellationToken token)
        {
            await Awaitable.BackgroundThreadAsync();
            var json = JsonUtility.ToJson(profile, true);
            await File.WriteAllTextAsync(_savePath, json, token);
            await Awaitable.MainThreadAsync();
        }

        public async Awaitable<Profile> LoadAsync(CancellationToken token)
        {
            if (!File.Exists(_savePath))
                return Profile.GetDefault();
            
            await Awaitable.BackgroundThreadAsync();
            var json = await File.ReadAllTextAsync(_savePath, token);
            var metagame = JsonUtility.FromJson<Profile>(json);
            await Awaitable.MainThreadAsync();
            
            return metagame;
        }
    }
}