using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public SpriteRenderer sr;
    public GameObject cameara;

    public float speed;

    float camaeraLength;
    float length;
    void Start()
    {
        length= sr.sprite.bounds.size.x* transform.localScale.x;
        camaeraLength = 30.1f;
        Debug.Log(length+gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        Vector2 bPos = transform.position;
        Vector2 aPos = new Vector3(speed*h, 0,0) * Time.deltaTime;
        transform.position = aPos + bPos;
        if (transform.position.x-cameara.transform.position.x>length*2)
        {
            var pos = transform.position;
            pos.x += length * -4;
            transform.position = pos;
           
        }
        else if (transform.position.x - cameara.transform.position.x < length *-2)
        {
            var pos = transform.position;
            pos.x += length*4;
            transform.position = pos;
        }
    }
}
