using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Flip(float value)
    {
        _renderer.flipX = value switch
        {
            < 0 => true,
            > 0 => false,
            _ => _renderer.flipX
        };
    }
}
