using UnityEngine;

public class Smoking : MonoBehaviour
{
    public float speed;
    int _i;
    float _a;
    bool _isMove;

    Player _player;
    Animator _animator;
    Rigidbody2D _rigid;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _animator = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        _rigid.gravityScale = 0;
    }

    private void Update()
    {
        var direction = _player.transform.position.x - transform.position.x;
        _i = direction switch
        {
            > 0 => 1,
            < 0 => -1,
            _ => _i
        };

        var aPos = new Vector2(speed * _i, 0) * Time.deltaTime;
        var bPos = (Vector2) transform.position;
        if (_isMove)
        {
            Debug.Log("kjsdfshe");
            transform.position = bPos + aPos;
        }
    }

    public void Move()
    {
        Debug.Log("무브");
        _animator.SetBool("isMove", true);
        _isMove = true;
        Jump();
        _rigid.gravityScale = 0.5f;
    }

    public void End()
    {
        Destroy(gameObject);
    }

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
        _a = Random.Range(2, 6);
        _rigid.AddForce(Vector2.up * _a, ForceMode2D.Impulse);
    }
}