using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public GameObject[] point;
    public GameObject lightningEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator StartLightning()
    {
        for (int i = 0; i < point.Length; i++)
        {
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            Instantiate(lightningEffect, point[i].transform.position, Quaternion.identity);
        }
       
    }
}
