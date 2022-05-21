using UnityEngine;

public class BackgroundLayerMover : MonoBehaviour
{
    public Transform target;
    public float multiplier = 1;

    private Vector3 _origin;
    
    private void Start()
    {
        _origin = transform.position;
    }

    private void Update()
    {
        var pos = (_origin + target.position) * (-1 * multiplier);
        pos.z = 0;
        transform.position = pos;
    }
}
