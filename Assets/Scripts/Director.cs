using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Director : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GameManager.Instance.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NewGame()
    {
        SceneManager.LoadScene("PlayScene"); 
    }
    public void Seting()
    {
        SceneManager.LoadScene("SettingScene" , LoadSceneMode.Additive);
    }
}
