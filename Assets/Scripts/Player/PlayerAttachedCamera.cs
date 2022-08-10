using UnityEngine;

public class PlayerAttachedCamera : MonoBehaviour
{
    public float maxX = 115.6f, minX = 2.4f;
    public float maxY = 0.2f, minY = -1.6f;

    public GameObject backgroundSprite;

    public bool isIn;

    private Player _player;


    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        var position = _player.transform.position;
        if (isIn)
        {
            position.x = Mathf.Clamp(position.x, minX, maxX);
            position.y = Mathf.Clamp(position.y, minY, maxY);
            backgroundSprite.SetActive(true);
        }
        else
        {
            backgroundSprite.SetActive(false);
        }
        
        transform.position = new Vector3(position.x, position.y, -10);
    }
}