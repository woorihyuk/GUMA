using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class YieldlnstructionCache
{
    class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y)
        {
            return x == y;
        }
        int IEqualityComparer<float>.GetHashCode(float obj)
        {
            return obj.GetHashCode();
        }
    }
    public static readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    private static readonly Dictionary<float, WaitForSeconds> _timelnterval = new Dictionary<float, WaitForSeconds>(new FloatComparer());
    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        if (!_timelnterval.TryGetValue(seconds, out wfs))
            _timelnterval.Add(seconds, wfs = new WaitForSeconds(seconds));
        return wfs;
    }
}

public class GameManager : MonoBehaviour
{
    public int savePoint;

    
    public static GameManager instance; 
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<GameManager>();
                if (obj!=null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<GameManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    // Start is called before the first frame update

    private void Awake()
    {
        var objs = FindObjectsOfType<GameManager>();
        if (objs.Length!=1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        GameLoad();
    }
    void Start()
    {
        GameLoad();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GameSave()
    {
        PlayerPrefs.SetInt("SavePoint", GameManager.Instance.savePoint);
        PlayerPrefs.Save();
    }
    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("SavePoint"))
        {
            return;
        }
        savePoint=PlayerPrefs.GetInt("SavePoint");
        Debug.Log("gameLoad");
    }
}

