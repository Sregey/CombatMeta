using UnityEngine;

namespace SaberCombatMeta.UI
{
    public class CanvasToCamera: MonoBehaviour
    {
        private void LateUpdate()
        {
            var camera = Camera.main;
            if (camera)
            {
                transform.forward = camera.transform.forward;
            }
        }
    }
}