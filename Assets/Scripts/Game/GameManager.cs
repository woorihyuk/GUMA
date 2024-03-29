using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int savePoint;
    public int positionFlags = -1;
    public float lastDirection;
    public Vector3 lastPosition;
    public bool isEndWatched;
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
    
    public void SaveGame(int pointFlag, string levelName, float direction)
    {
        PlayerPrefs.SetInt("SavePointFlag", pointFlag);
        PlayerPrefs.SetString("SavePointLevel", levelName);
        PlayerPrefs.SetFloat("SaveLastDirection", direction);
        PlayerPrefs.Save();
        Debug.Log("[GameManager] Game Saved");
    }

    public bool CheckIsLoaded()
    {
        if (!_isSaveLoaded) return false;
        _isSaveLoaded = false;
        return true;
    }
    
    public bool LoadGame()
    {
        if (!PlayerPrefs.HasKey("SavePointFlag")) return false;
        if (!PlayerPrefs.HasKey("SavePointLevel")) return false;

        var flag = PlayerPrefs.GetInt("SavePointFlag");
        var level = PlayerPrefs.GetString("SavePointLevel");
        var direction = PlayerPrefs.GetFloat("SaveLastDirection");

        savePoint = flag;
        lastDirection = direction;
        _isSaveLoaded = true;
        Debug.Log("[GameManager] Save Loaded");
        SceneManager.LoadScene(level);
        return true;
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