using UnityEngine;
using UnityEngine.SceneManagement;

public class Director : MonoBehaviour
{
    public void NewGame()
    {
        GameManager.Instance.savePoint = 0;
        SceneManager.LoadScene("PlayScene");
    }

    public void LoadGame()
    {
        GameManager.Instance.GameLoad();
        SceneManager.LoadScene("PlayScene");
    }

    public void Settings()
    {
        SceneManager.LoadScene("SettingScene", LoadSceneMode.Additive);
    }

    public void End()
    {
        Application.Quit();
    }
}