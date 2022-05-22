using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Setting
{
    public class SettingManager : MonoBehaviour
    {
        public string returnSceneName;

        private JangseungCtrl jangseungCtrl;
        //private SettingManager settingManager;
        public static SettingManager settingManager;

        private static bool isPause = false;
        
        public void Awake()
        {
            settingManager = this;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SwitchPause();
            }
        }

        public void SwitchPause()
        {
            isPause = !isPause;

            if (isPause)
            {
                settingManager.Pause();
                StartCoroutine(Init());
            }
            else
            {
                jangseungCtrl.StartAni();
            }
        }

        public void SwitchPause(bool state)
        {
            isPause = state;

            if (isPause)
            {
                settingManager.Pause();
                StartCoroutine(Init());
            }
            else
            {
                jangseungCtrl.StartAni();
            }
        }

        public void Pause()
        {
            Time.timeScale = 0;

            SceneManager.LoadScene("SettingScene", LoadSceneMode.Additive);
        }

        public void UnPause()
        {
            StartCoroutine(Unload());
        }

        private IEnumerator Init()
        {
            yield return null;

            jangseungCtrl = GameObject.FindObjectOfType<JangseungCtrl>();
        }

        private static IEnumerator Unload()
        {
            var async = SceneManager.UnloadSceneAsync("SettingScene");

            while (!async.isDone)
            {
                yield return null;
            }

            Time.timeScale = 1;
        }
    }
}