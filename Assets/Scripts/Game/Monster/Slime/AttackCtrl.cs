using UnityEngine;

public class AttackCtrl : MonoBehaviour
{
    public GameObject player;
    public GameObject attack;

    private Vector3 _attackPos;
    private Animator _anim;
    private SpriteRenderer _sr;
    private bool _isGround;
    private static readonly int IsAttack = Animator.StringToHash("isAttack");

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _sr.color = new Color(1, 1, 1, 0);
    }

    private void Update()
    {
        var bPos = (Vector2)transform.position;
        var aPos = new Vector2(0, -70) * Time.deltaTime;
        if (!_isGround)
        {
            transform.position = bPos + aPos;
        }
    }

    public void Move(int i)
    {
        transform.position = new Vector3(player.transform.position.x, 20, 0);
        _isGround = false;
        transform.rotation = Quaternion.Euler(0, i == -1 ? 0 : 180, 0);
    }

    public void Attack()
    {
        _sr.color = new Color(1, 1, 1, 1);
        _anim.SetBool(IsAttack, true);
    }

    public void AttackRange()
    {
        Instantiate(attack, transform.position, Quaternion.identity);
    }

    public void AttackEnd()
    {
        _anim.SetBool(IsAttack, false);
        _sr.color = new Color(1, 1, 1, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            _isGround = true;
        }
    }
}