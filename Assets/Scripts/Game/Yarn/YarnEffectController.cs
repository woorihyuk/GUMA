using DG.Tweening;
using UnityEngine;
using Yarn.Unity;

namespace Game.Yarn
{
    public class YarnEffectController : MonoBehaviour
    {
        [YarnCommand("camera_look")]
        private void CameraLookAtTarget(GameObject target)
        {
            if (target == null)
            {
                Debug.LogError("Can't find the target!");
                return;
            }
            
            // UnityEngine.Camera.main!.transform.DOMoveX()
        }
    }
}