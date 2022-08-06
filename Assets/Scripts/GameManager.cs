using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int savePoint;
    public int positionFlags = -1;
    private bool _isSaveLoaded;

    protected override void Awake()
    {
        base.Awake();
        GameLoad();
    }

    public void GameSave()
    {
        PlayerPrefs.SetInt("SavePoint", Instance.savePoint);
        PlayerPrefs.Save();
    }
    
    public void SaveGame(int pointFlag, string levelName)
    {
        PlayerPrefs.SetInt("SavePointFlag", pointFlag);
        PlayerPrefs.SetString("SavePointLevel", levelName);
        PlayerPrefs.Save();
        Debug.Log("[GameManager] Game Saved");
    }

    public bool CheckIsLoaded()
    {
        if (!_isSaveLoaded) return false;
        _isSaveLoaded = false;
        return true;
    }
    
    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("SavePointFlag")) return;
        if (!PlayerPrefs.HasKey("SavePointLevel")) return;

        var flag = PlayerPrefs.GetInt("SavePointFlag");
        var level = PlayerPrefs.GetString("SavePointLevel");

        savePoint = flag;
        _isSaveLoaded = true;
        Debug.Log("[GameManager] Save Loaded");
        SceneManager.LoadScene(level);
    }

    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("SavePoint"))
        {
            return;
        }

        savePoint = PlayerPrefs.GetInt("SavePoint");
    }
}