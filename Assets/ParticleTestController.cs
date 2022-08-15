using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Pool;

public class ParticleTestController : MonoBehaviour
{
    public IObjectPool<ParticleTest> testPool;
    public ParticleTest prefab;
    public List<Sprite> sprites;
    public Transform mainC;
    public bool isSub;

    private void Start()
    {
        testPool = new ObjectPool<ParticleTest>(() => Instantiate(prefab)
            , test =>
            {
                test.gameObject.SetActive(true);
                test.sprites = sprites;
                if (isSub) transform.localPosition = new Vector3(-mainC.localPosition.x, mainC.localPosition.y);
                test.transform.position = transform.position;
                test.controller = this;
                test.transform.localScale = new Vector3(1, 1, 1);
            }, test =>
                test.gameObject.SetActive(false), test => Destroy(test.gameObject)
        );

        Observable.IntervalFrame(4, FrameCountType.EndOfFrame)
            .Do(_ => { testPool.Get(); }).Subscribe().AddTo(gameObject);
    }
    
    [Header("속도, 길이")]
    [SerializeField] [Range(0f,10f)] private float speed = 1f;
    [SerializeField] [Range(0f,10f)]  private float length = 1f;
 
    private float runningTime = 0f;
    private float xPos = 0f;

    private void Update()
    {
        if (!isSub)
        {
            runningTime += Time.deltaTime * speed;
            xPos = Mathf.Sin(runningTime) * length;
            transform.localPosition = new Vector2(xPos,transform.localPosition.y);
        }
    }
}