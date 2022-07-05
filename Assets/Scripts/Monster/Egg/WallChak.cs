using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChak : MonoBehaviour
{
    Egg egg;
    
    // Start is called before the first frame update
    void Start()
    {
        egg = FindObjectOfType<Egg>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            print("충돌");
            egg.isWall=true;
        }
    }
}
