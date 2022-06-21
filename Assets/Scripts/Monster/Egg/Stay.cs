using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stay : MonoBehaviour
{
    Egg egg;
    // Start is called before the first frame update
    void Start()
    {
        egg = GetComponent<Egg>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator Wait(float i)
    {
        yield return new WaitForSeconds(i);
        yield return null;
    }
}
