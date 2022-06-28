using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeauniCtrl : MonoBehaviour
{
    public GameObject[] movePoint;

    public float attackRange;

    private Player _player;

    private float _direction;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        var dist = Vector2.Distance(_player.transform.position, transform.position);
        _direction = _player.transform.position.x - transform.position.x;
        if (dist<attackRange)
        {
            switch (_direction)
            {
                case > 0:

                    break;
                case < 0:
                    break;
            }
        }
    }
}
