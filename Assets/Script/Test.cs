using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float speed;
    int i;

    SpriteRenderer sr;
    PlayerCtrl player;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerCtrl>();
        //StartCoroutine(LookPlayer());
        //transform.DOMove(new Vector3(0, transform.position.y), 8).SetEase(Ease.Linear);
    }


    /*IEnumerator LookPlayer()
    {

        while (true)
        {
            var pos = transform.position.x - player.transform.position.x;
            Debug.Log(pos);
            if (pos > 0)
            {
                if(sr.flipX)
                {
                    sr.flipX = false;
                    yield return new WaitForSeconds(2);
                }
            }
            else if (pos < 0)
            {
                if(!sr.flipX)
                {
                    sr.flipX = true;
                    yield return new WaitForSeconds(2);
                }
            }
            Vector2 bPos = transform.position;
            Vector2 aPos = new Vector2(speed, 0) * i * Time.deltaTime;
            transform.position = bPos + aPos;
            yield return null;
        }
    }*/
}
