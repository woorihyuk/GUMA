using UnityEngine;

public class PlayerAttachedCamera : MonoBehaviour
{
    public float maxX = 115.6f, minX = 2.4f;

    public float maxY = 0.2f;
    public float minY = -1.6f;

    public GameObject backGround;

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
            backGround.SetActive(true);
        }
        else
        {
            backGround.SetActive(false);
        }
        

        transform.position = new Vector3(position.x, position.y, -10);
    }
}