using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerController : MonoBehaviour
{
    private const float AccelerationTimeAirborne = 0.2f;
    private const float AccelerationTimeGrounded = 0.1f;
    
    public float jumpHeight = 6;
    public float timeToJumpApex = 0.4f;
    public float moveSpeed = 8;

    private Controller2D _controller;
    private Vector2 _input, _velocity;
    private float _gravity, _jumpVelocity, _velocityXSmoothing;

    private void Start()
    {
        _controller = GetComponent<Controller2D>();
        _gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        _jumpVelocity = Mathf.Abs(_gravity) * timeToJumpApex;
    }

    private void Update()
    {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if (_controller.collisions.above || _controller.collisions.below)
        {
            _velocity.y = 0;
        }
        
        var targetVelocityX = _input.x * moveSpeed;
        var gravityMultiplier = 1f;

        if (Input.GetKeyDown(KeyCode.Space)) Jump();

        _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing,
            _controller.collisions.below ? AccelerationTimeGrounded : AccelerationTimeAirborne);
        _velocity.y += _gravity * Time.deltaTime * gravityMultiplier;
        
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void Jump()
    {
        _velocity.y = _jumpVelocity;
    }
}
