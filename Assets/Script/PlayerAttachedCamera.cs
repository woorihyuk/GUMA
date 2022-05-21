using System;
using UnityEngine;

public class PlayerAttachedCamera : MonoBehaviour
{
    public float maxY = 0.2f;

    private Player _player;
    
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        var position = _player.transform.position;
        if (position.x <= 2.4f)
        {
            position.x = 2.4f;
        }

        if (position.x >= 115.6f)
        {
            position.x = 115.6f;
        }

        if (position.y >= maxY)
        {
            position.y = maxY;
        }

        transform.position = new Vector3(position.x, position.y, -10);
    }
}