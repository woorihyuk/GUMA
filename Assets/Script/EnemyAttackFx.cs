using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackFx : MonoBehaviour
{
    float attackTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackTime += Time.deltaTime;
        if (attackTime >= 0.01)
        {
            Destroy(gameObject);
        }
    }
}
