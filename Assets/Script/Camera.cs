using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject player;
    float x;
    float y;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        x = player.transform.position.x;
        y = player.transform.position.y;
        if (x<=2.4)
        {
            x = transform.position.x;
        }
        if (x>= 115.6)
        {
            x = transform.position.x;
        }
        if (y>=6.5)
        {
            y = transform.position.y;
        }
        transform.position = new Vector3(x, y, -10);
    }
}
