using System.Collections.Generic;
using UnityEngine;

internal static class YieldInstructionCache
{
    class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y)
        {
            return x.Equals(y);
        }

        int IEqualityComparer<float>.GetHashCode(float obj)
        {
            return obj.GetHashCode();
        }
    }

    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new();

    private static readonly Dictionary<float, WaitForSeconds> TimeInterval = new(new FloatComparer());
    private static readonly Dictionary<float, WaitForSecondsRealtime> TimeIntervalRealtime = new(new FloatComparer());


    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if (!TimeInterval.TryGetValue(seconds, out var wfs))
            TimeInterval.Add(seconds, wfs = new WaitForSeconds(seconds));
        return wfs;
    }
    
    public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds)
    {
        if (!TimeIntervalRealtime.TryGetValue(seconds, out var wfs))
            TimeIntervalRealtime.Add(seconds, wfs = new WaitForSecondsRealtime(seconds));
        return wfs;
    }
}

public class GameManager : MonoBehaviour
{
    public int savePoint;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = FindObjectOfType<GameManager>();
                if (obj != null)
                {
                    _instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<GameManager>();
                    _instance = newObj;
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
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