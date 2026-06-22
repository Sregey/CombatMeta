using UnityEngine.SceneManagement;

namespace SaberCombatMeta.Common
{
    public static class SceneExtensions
    {
        public static T FindComponent<T>(this ref Scene scene)
        {
            foreach (var rootObject in scene.GetRootGameObjects())
            {
                if (rootObject.TryGetComponent<T>(out var component))
                {
                    return component;
                }
            }
            return default;
        }
    }
}