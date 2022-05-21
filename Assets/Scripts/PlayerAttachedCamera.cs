using UnityEngine;

public class PlayerAttachedCamera : MonoBehaviour
{
    public float maxX = 115.6f, minX = 2.4f;

    public float maxY = 0.2f;
    public float minY = -1.6f;

    private Player _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        var position = _player.transform.position;
        
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        transform.position = new Vector3(position.x, position.y, -10);
    }
}