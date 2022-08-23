using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LightningAnim());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LightningAnim()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.01f);
        Destroy(gameObject);
    }
}
