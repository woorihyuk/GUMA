using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterMove : MonoBehaviour
{
    public float HP;
    public float speed;
    public float foundRange;
    public float waitTime;
    public float maxX;
    public float minX;
    public float attackRange;
    public float dist;

    public int _i;
    private int _x;

    public float _direction;

    public bool _isWait;
    public bool _isMove;
    public bool _isWalk;
    public bool _isFound;
    public bool _isAttack;
    public bool doAttack;

    public Vector2 _aPos;
    public Vector2 _bPos;

    public GameUIManager _gameUIManager;
    public Player _player;
    public Animator _animator;

    //public static readonly int AnimIsWalk;
    //public static readonly int AnimIsDie;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _gameUIManager = FindObjectOfType<GameUIManager>();
        _player = FindObjectOfType<Player>();
        _animator = GetComponent<Animator>();
        _i = 1;
        _isMove = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        dist = Vector2.Distance(transform.position, _player.transform.position);
        _direction = _player.transform.position.x - transform.position.x;
        _bPos = transform.position;
        _aPos = new Vector2(speed * _i, 0) * Time.deltaTime;
        print(transform.position);
        //RefreshHp(HP);

        if (dist < foundRange)
        {
            _isFound = true;
        }
        else
        {
            if (_isAttack)
            {
                _isFound = false;
            }
        }


        if (!_isFound)
        {
            //_gameUIManager.TryPopHpBar(GetInstanceID().ToString());
            if (_isMove)
            {
                //_animator.SetBool(AnimIsWalk, true);
                transform.position = _bPos + _aPos;
                print("a");
                if (transform.position.x>=maxX)
                {
                    if (_i==1)
                    {
                        _isWait = true;
                        _x = -1;
                    }
                }
                else if (transform.position.x <= minX)
                {
                    if (_i == -1)
                    {
                        _isWait = true;
                        _x = 1;
                    }
                }
            }

            if (_isWait)
            {
                base.StartCoroutine(Wait(waitTime, _x));
            }
        }
        if (_isFound)
        {
            if (dist > attackRange)
            {
                if (!_isAttack)
                {
                    switch (_direction)
                    {
                        case < 0:
                            _i = -1;
                            break;
                        case > 0:
                            _i = 1;
                            break;
                    }
                    transform.position = _aPos + _bPos;
                }
            }
            else
            {
                doAttack = true;
            }
        }

        if (HP <= 0)
        {
            //_animator.SetBool(AnimIsDie, true);
        }
    }

    protected virtual IEnumerator Wait(float waitTime, int directon)
    {
        _isWait = false;
        _isMove = false;
        //_animator.SetBool(AnimIsWalk, false);
        yield return YieldInstructionCache.WaitForSeconds(waitTime);
        _i = directon;
        _isMove = true;
    }

    //IEnumerator Walk(float walkTime)
    //{
    //    _isWalk = true;
    //    yield return YieldInstructionCache.WaitForSeconds(walkTime);
    //    _isMove = false;
    //    _isWait = true;
    //    _isWalk = false;
    //}

    //private void RefreshHp(float newHp)
    //{
    //    if (!newHp.Equals(_lastHp))
    //    {
    //        _gameUIManager.SetHpBarPercent(GetInstanceID().ToString(), newHp / _mxHp);
    //        _lastHp = newHp;
    //    }
    //}
}
