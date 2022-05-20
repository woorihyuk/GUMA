using UnityEngine;

public class PlayerAttachedCamera : MonoBehaviour
{
    public GameObject player;
    public float maxY = 0.2f;

    private void Update()
    {
        var x = player.transform.position.x;
        var y = player.transform.position.y;
        if (x <= 2.4)
        {
            x = transform.position.x;
        }

        if (x >= 115.6)
        {
            x = transform.position.x;
        }

        if (y >= maxY)
        {
            y = transform.position.y;
        }

        transform.position = new Vector3(x, y, -10);
    }
}