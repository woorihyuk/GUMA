using Game.Player;
using UnityEngine;

public class Smoking : MonoBehaviour
{
    public float speed = 2.5f;
    private int _intDirection;
    private float _direction;
    private bool _isMove;

    private Player _player;
    private Animator _animator;
    private Rigidbody2D _rigid;
    private static readonly int IsMove = Animator.StringToHash("isMove");

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _animator = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        _rigid.gravityScale = 0;
        speed = Random.Range(1, 7);
    }

    private void Update()
    {
        var curPosVector2 = (Vector2) transform.position;
        _direction = _player.transform.position.x - curPosVector2.x;

        var aPos = new Vector2(speed * _intDirection, 0) * Time.deltaTime;
        if (_isMove) transform.position = curPosVector2 + aPos;
    }

    public void Move()
    {
        // Debug.Log("무브");
        _animator.SetBool(IsMove, true);
        _isMove = true;
        Jump();
        _rigid.gravityScale = 0.5f;
    }

    //public void End()
    //{
    //    Destroy(gameObject);
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ground"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    private void Jump()
    {
        _intDirection = _direction switch
        {
            > 0 => 1,
            < 0 => -1,
            _ => _intDirection
        };
        _rigid.AddForce(Vector2.up * Random.Range(2, 10), ForceMode2D.Impulse);
    }
}