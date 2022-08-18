using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Setting
{
    public class JangseungCtrl : MonoBehaviour
    {
        private static Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void StopAni()
        {
            animator.speed = 0;
        }

        public void StartAni()
        {
            animator.speed = 1;
        }

        public void OutScene()
        {
            SettingManager.settingManager.UnPause();
        }

        public void End()
        {
            SceneManager.LoadScene("Title");
        }
    }
}