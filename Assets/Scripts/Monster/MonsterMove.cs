using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class MonsterMove : MonoBehaviour
{
    private Collider2D _collider;
    private readonly List<Collider2D> _colliders = new();
    private ContactFilter2D _filter;
    public LayerMask contactLayerMask;

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
        _collider = GetComponentInChildren<BoxCollider2D>();
        _colliders.Clear();
        _filter = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = contactLayerMask
        };
    }

    protected virtual void Update()
    {
        var counts = _collider.OverlapCollider(_filter, _colliders);
        if (counts == 0) // 아무것도 못 찾았으면
        {
            _lastTargetPlayer = null;
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
            OnPlayerFound();
        }
    }

    public void Move(float speed, int direction)
    {
        var bPos = transform.position;
        var aPos = new Vector3(speed * direction, 0);
        transform.position = bPos + aPos;
    }

    public float maxX, minX, waitTime, speed;
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
            //Observable.Timer(TimeSpan.FromMilliseconds(waitTime)).Subscribe
        }
        Move(speed, i);
    }

    protected abstract void OnPlayerFound();
}
