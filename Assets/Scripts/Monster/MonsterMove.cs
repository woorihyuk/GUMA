using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class MonsterMove : MonoBehaviour
{
    private Collider2D _collider;
    private readonly List<Collider2D> _colliders = new();
    private ContactFilter2D _filter;
    public LayerMask contactLayerMask;
    public bool isPlayerFound;

    private TargetPlayerData _lastTargetPlayer;

    private class TargetPlayerData
    {
        public float distance;
        public Player player;

        public TargetPlayerData(float dst, Player player)
        {
            distance = dst;
            this.player = player;
        }
    }

    protected virtual void Start()
    {
        hp = maxHp;
        _collider = GetComponentsInChildren<BoxCollider2D>()[1];
        _colliders.Clear();
        _filter = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = contactLayerMask
        };
        spawnedPosition = transform.position;
    }

    protected virtual void Update()
    {
        var counts = _collider.OverlapCollider(_filter, _colliders);
        if (counts == 0) // 아무것도 못 찾았으면
        {
            _lastTargetPlayer = null;
            isPlayerFound = true;
            return;
        }
        
        foreach (var col in _colliders)
        {
            var player = col.GetComponent<Player>();

            if (_lastTargetPlayer == null) // 현재까지 찾은 플레이어가 없을 때
            {
                _lastTargetPlayer = new TargetPlayerData(Vector2.Distance(transform.position, player.transform.position), player);
            }
            else // 찾은 플레이어가 이미 있는데 또 다른 플레이어를 찾을 때
            {
                var dst = Vector2.Distance(transform.position, player.transform.position);
                if (_lastTargetPlayer.distance < dst) // 마지막으로 찾은 플레이어가 더 가까울때
                {
                    _lastTargetPlayer.distance = dst; // 거리 갱신
                    continue; // 반복문 다시 돌기
                }
                
                // 새로 찾은 플레이어가 더 가까울 때
                if (_lastTargetPlayer.player != player) // 이전에 찾은 플레이어가 지금 찾은 플레이어랑 다르면
                {
                    // 새 플레이어를 찾았을 때 동작
                }

                _lastTargetPlayer = new TargetPlayerData(dst, player); // 플레이어 데이터 갱신
            }
        }

        if (_lastTargetPlayer != null)
        {
            isPlayerFound = false;
            OnPlayerFound(_lastTargetPlayer.player);
        }
    }

    public void Move(float speed, int direction)
    {
        transform.position += new Vector3(speed * direction, 0);
    }

    public float maxX, minX, waitTime, speed;
    public Vector3 spawnedPosition;
    public int hp, maxHp, lastHp;

    protected abstract void OnHpDrown();

    protected abstract void OnDirectionSet(int direction);

    public void RefreshHp(int newHp)
    {
        if (!newHp.Equals(lastHp))
        {
            try
            {
                GameUIManager.Instance.SetHpBarPercent(GetInstanceID().ToString(), (float)newHp/maxHp);
            }
            catch
            {
                return;
            }
            lastHp = newHp;
            
            if (hp <= 0)
            {
                OnHpDrown();
            }
        }
    }
    
    public void MoveAround()
    {
        var i = 1;
        if (transform.position.x>=maxX)
        {
            i = -1;
        }
        if (transform.position.x <= minX)
        {
            i = 1;
        }
    }

    protected IEnumerator AIMove(float maxFreezeDelay, float minMoveTime, float maxMoveTime)
    {
        while (gameObject)
        {
            yield return new WaitForSeconds(Random.Range(0, maxFreezeDelay)); // 프리징

            // 방향 정하기
            int direction;
            if (transform.position.x >= spawnedPosition.x + maxX) // 오른쪽 끝에 도달했으면
            {
                direction = -1; // 왼쪽으로
                Debug.Log($"오른쪽 끝에 도달함 : {direction}");
            }
            else if (transform.position.x <= spawnedPosition.x - maxX) // 왼쪽 끝에 도달했으면
            {
                direction = 1; // 오른쪽으로
                Debug.Log($"왼쪽 끝에 도달함 : {direction}");
            }
            else // 그 중간에 있다면
            {
                direction = Random.value < 0.5f ? -1 : 1; // 랜덤으로 왼쪽 이동 or 오른쪽 이동
                Debug.Log($"중간에 위치함 : {direction}");
            }

            OnDirectionSet(direction);
            
            // 이동하기
            var moveTime = Random.Range(minMoveTime, maxMoveTime);
            Debug.Log($"이동 시작 : {moveTime}");
            var timer = 0f;
            
            while (moveTime > timer)
            {
                Move(speed * Time.deltaTime, direction);
                timer += Time.deltaTime;
                
                if (direction == -1)
                {
                    if (transform.position.x <= spawnedPosition.x - maxX)
                    {
                        Vector3 pos = transform.position;
                        pos.x = spawnedPosition.x - maxX;
                        transform.position = pos;
                    }
                }
                else if (transform.position.x >= spawnedPosition.x + maxX)
                {
                    Vector3 pos = transform.position;
                    pos.x = spawnedPosition.x + maxX;
                    transform.position = pos;
                }
                
                yield return null;
            }
            
            Debug.Log("이동 완료");

            yield return null;
        }
    }

    protected abstract void OnPlayerFound(Player player);
}
