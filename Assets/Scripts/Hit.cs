using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    PlayerCtrl player;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GetComponent<PlayerCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator HitAni()
    {
        player.isHit = true;
        sr.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.2f);
        sr.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.2f);
        sr.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.2f);
        sr.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.2f);
        sr.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.2f);
        sr.color = new Color(1, 1, 1, 1);
        player.isHit = false;
        yield return null;
    }
}
