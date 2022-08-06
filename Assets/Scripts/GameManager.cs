using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int savePoint;
    public int positionFlags = -1;

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

    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("SavePoint"))
        {
            return;
        }

        savePoint = PlayerPrefs.GetInt("SavePoint");
        Debug.Log("[GameManager] Save Loaded");
    }
}