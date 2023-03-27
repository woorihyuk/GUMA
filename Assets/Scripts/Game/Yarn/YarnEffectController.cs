using UnityEngine;
using Yarn.Unity;

namespace Game.Yarn
{
    public class YarnEffectController : MonoBehaviour
    {
        [YarnCommand("camera_look")]
        private void CameraLookAtTarget(GameObject target)
        {
            if (target == null) {
                Debug.Log("Can't find the target!");
            }
            
            // Make the main camera look at this target
            if (UnityEngine.Camera.main != null) UnityEngine.Camera.main.transform.LookAt(target.transform);
            else Debug.LogError("Can't find the MainCamera!");
        }    
    }
}