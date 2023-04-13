using System;
using DG.Tweening;
using UnityEngine;
using Yarn.Unity;

namespace Game.Yarn
{
    public class YarnEffectController
    {
        private void Awake()
        {
            // UnityEngine.Object.FindObjectOfType<DialogueRunner>().AddCommandHandler<GameObject>("camera_look", CameraLookAtTarget);
        }

        [YarnCommand("camera_look")]
        private static void CameraLookAtTarget(GameObject target)
        {
            if (target == null)
            {
                Debug.LogError("Can't find the target!");
                return;
            }
            
            Debug.Log(target.name);
            // UnityEngine.Camera.main!.transform.DOMoveX()
        }

        [YarnCommand("fade_in_image")]
        private static void FadeIn()
        {
            Debug.Log("fade in");
        }
    }
}