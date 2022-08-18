using UnityEngine;
using UnityEngine.SceneManagement;

public class Director : MonoBehaviour
{
    public void NewGame()
    {
        GameManager.Instance.savePoint = -1;
        SceneManager.LoadScene("01");
    }

    public void LoadGame()
    {
        // GameManager.Instance.GameLoad();
        GameManager.Instance.LoadGame();
        // SceneManager.LoadScene("PlayScene");
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