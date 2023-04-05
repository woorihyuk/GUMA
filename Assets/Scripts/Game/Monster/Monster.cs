using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Monster
{
    public abstract class Monster : MonoBehaviour
    {
        private Collider2D _collider;
        private readonly List<Collider2D> _colliders = new();
        private ContactFilter2D _filter;
        public LayerMask contactLayerMask;
        private int _groundLayerMask, _worldLayerMask;
        private Collider2D _mainCollider;
        public BoolReactiveProperty isPlayerFounded, isMonsterMoving;
        public Collider2D hitBoxCollider;
        protected IDisposable PlayerFoundSubscription;

        protected TargetPlayerData lastTargetPlayer;

        protected class TargetPlayerData
        {
            public float distance;
            public readonly Player.Player player;

            public TargetPlayerData(float dst, Player.Player player)
            {
                distance = dst;
                this.player = player;
            }
        }

        protected virtual void Start()
        {
            hp.Value = maxHp.Value;
            _collider = GetComponentsInChildren<BoxCollider2D>()[1];
            _colliders.Clear();
            _filter = new ContactFilter2D
            {
                useLayerMask = true,
                layerMask = contactLayerMask
            };
            spawnedPosition = transform.position;

            PlayerFoundSubscription = isPlayerFounded.DistinctUntilChanged().Subscribe(v =>
            {
                if (!v) OnPlayerLost();
                else OnPlayerFound();
            }).AddTo(gameObject);

            hp.DistinctUntilChanged().Subscribe(_ => { RefreshHp(); }).AddTo(gameObject);

            _groundLayerMask = LayerMask.GetMask("WorldGround");
            _worldLayerMask = LayerMask.GetMask("WorldNotGround", "WorldGround");
            _mainCollider = GetComponent<BoxCollider2D>();
        }

        protected virtual void Update()
        {
            var counts = _collider.OverlapCollider(_filter, _colliders);
            if (counts == 0) // 아무것도 못 찾았으면
            {
                lastTargetPlayer = null;
                isPlayerFounded.Value = false;
                return;
            }

            foreach (var col in _colliders)
            {
                var player = col.GetComponent<Player.Player>();

                if (lastTargetPlayer == null) // 현재까지 찾은 플레이어가 없을 때
                {
                    lastTargetPlayer =
                        new TargetPlayerData(Vector2.Distance(transform.position, player.transform.position), player);
                }
                else // 찾은 플레이어가 이미 있는데 또 다른 플레이어를 찾을 때
                {
                    var dst = Vector2.Distance(transform.position, player.transform.position);
                    if (lastTargetPlayer.distance < dst) // 마지막으로 찾은 플레이어가 더 가까울때
                    {
                        lastTargetPlayer.distance = dst; // 거리 갱신
                        continue; // 반복문 다시 돌기
                    }

                    // 새로 찾은 플레이어가 더 가까울 때
                    if (lastTargetPlayer.player != player) // 이전에 찾은 플레이어가 지금 찾은 플레이어랑 다르면
                    {
                        // 새 플레이어를 찾았을 때 동작
                    }

                    lastTargetPlayer = new TargetPlayerData(dst, player); // 플레이어 데이터 갱신
                }
            }

            if (lastTargetPlayer != null)
            {
                isPlayerFounded.Value = true;
            }
        }

        private void Move(float moveSpeed, int direction)
        {
            transform.position += new Vector3(moveSpeed * direction, 0);
        }

        public float maxX, speed;
        public Vector3 spawnedPosition;
        public IntReactiveProperty hp, maxHp;

        protected abstract void OnHpDrown();

        protected abstract void OnDirectionSet(int direction);

        public virtual void OnMonsterGetDamaged(int dmg)
        {
            hp.Value -= dmg;
            
            FxPoolManager.Instance.damageTextPool.Get(out var vText);
            var yMax = hitBoxCollider.bounds.max.y;
            var currentPos = transform.position;

            vText.transform.position = new Vector3(currentPos.x + Random.Range(-1f, 1f),
                yMax + Random.Range(-1.5f, 1f), currentPos.z);
            vText.SetText(dmg);
            vText.Play();
        }

        protected void RefreshHp()
        {
            try
            {
                GameUIManager.Instance.SetHpBarPercent(GetInstanceID().ToString(), (float)hp.Value / maxHp.Value);
            }
            catch
            {
                //
            }

            if (hp.Value <= 0)
            {
                OnHpDrown();
            }
        }

        protected IEnumerator AIMove(float minFreezeDelay, float maxFreezeDelay, float minMoveTime, float maxMoveTime)
        {
            while (gameObject)
            {
                isMonsterMoving.Value = false;
                yield return new WaitForSeconds(Random.Range(minFreezeDelay, maxFreezeDelay)); // 프리징

                // 방향 정하기
                int direction;
                if (transform.position.x >= spawnedPosition.x + maxX) // 오른쪽 끝에 도달했으면
                {
                    direction = -1; // 왼쪽으로
                    // Debug.Log($"오른쪽 끝에 도달함 : {direction}");
                }
                else if (transform.position.x <= spawnedPosition.x - maxX) // 왼쪽 끝에 도달했으면
                {
                    direction = 1; // 오른쪽으로
                    // Debug.Log($"왼쪽 끝에 도달함 : {direction}");
                }
                else // 그 중간에 있다면
                {
                    direction = Random.value < 0.5f ? -1 : 1; // 랜덤으로 왼쪽 이동 or 오른쪽 이동
                    // Debug.Log($"중간에 위치함 : {direction}");
                }

                if (!IsGroundForward(direction))
                {
                    direction *= -1;
                }
                else if (!IsNoWallForward(direction))
                {
                    direction *= -1;
                }
                OnDirectionSet(direction);

                // 이동하기
                var moveTime = Random.Range(minMoveTime, maxMoveTime);
                // Debug.Log($"이동 시작 : {moveTime}");
                isMonsterMoving.Value = true;
                var timer = 0f;
                var tf = transform;

                while (moveTime > timer)
                {
                    timer += Time.deltaTime;
                    if (!IsGroundForward(direction)) yield return null;
                    else if (!IsNoWallForward(direction)) yield return null;
                    else
                    {
                        Move(speed * Time.deltaTime, direction);

                        switch (direction)
                        {
                            case < 0:
                            {
                                if (tf.position.x <= spawnedPosition.x - maxX)
                                {
                                    var pos = tf.position;
                                    pos.x = spawnedPosition.x - maxX;
                                    tf.position = pos;
                                }

                                break;
                            }
                            case > 0:
                            {
                                if (tf.position.x >= spawnedPosition.x + maxX)
                                {
                                    var pos = tf.position;
                                    pos.x = spawnedPosition.x + maxX;
                                    tf.position = pos;
                                }

                                break;
                            }
                        }
                        yield return null;
                    }
                }

                // Debug.Log("이동 완료");
                yield return null;
            }
        }

        public bool IsGroundForward(int direction)
        {
            switch (direction)
            {
                case < 0:
                {
                    var bounds = _mainCollider.bounds;
                    var vector = new Vector2(bounds.min.x - 0.1f, bounds.min.y - 0.2f);
                    var ground = Physics2D.OverlapPoint(vector, _groundLayerMask);
                    return ground;
                }
                case > 0:
                {
                    var bounds = _mainCollider.bounds;
                    var vector = new Vector2(bounds.max.x - 0.1f, bounds.min.y - 0.2f);
                    var ground = Physics2D.OverlapPoint(vector, _groundLayerMask);
                    return ground;
                }
                default:
                    throw new ArgumentException("direction은 0이 될 수 없습니다");
            }
        }

        public bool IsNoWallForward(int direction)
        {
            switch (direction)
            {
                case < 0:
                {
                    var bounds = _mainCollider.bounds;
                    var vector1 = new Vector2(bounds.min.x, bounds.min.y + 0.1f);
                    var vector2 = new Vector2(bounds.min.x - 0.05f, bounds.max.y);
                    var wall = Physics2D.OverlapArea(vector1, vector2, _worldLayerMask);
                    return !wall;
                }
                case > 0:
                {
                    var bounds = _mainCollider.bounds;
                    var vector1 = new Vector2(bounds.max.x, bounds.min.y + 0.1f);
                    var vector2 = new Vector2(bounds.max.x + 0.05f, bounds.max.y);
                    var wall = Physics2D.OverlapArea(vector1, vector2, _worldLayerMask);
                    return !wall;
                }
                default:
                    throw new ArgumentException("direction은 0이 될 수 없습니다");
            }
        }

        protected abstract void OnPlayerLost();

        protected abstract void OnPlayerFound();
        
        protected void StartCoroutineWithRunningCheck(ref Coroutine lastCoroutine, IEnumerator newRoutine)
        {
            if (lastCoroutine != null) StopCoroutine(lastCoroutine);
            lastCoroutine = StartCoroutine(newRoutine);
        }
    }
}