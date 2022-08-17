using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private float attackTime;
    
    void Update()
    {
        attackTime += Time.deltaTime;
        if (attackTime >= 0.01f)
        {
            Destroy(gameObject);
        }
    }
}
