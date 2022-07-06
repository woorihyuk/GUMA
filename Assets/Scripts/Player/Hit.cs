using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public GameObject saveMessage;

    Player player;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator HitAni()
    {
        Debug.Log("맞음");
        player.isHit = false;
        sr.color = new Color(1, 1, 1, 0);
        yield return YieldlnstructionCache.WaitForSeconds(0.15f);
        sr.color = new Color(1, 1, 1, 1);
        yield return YieldlnstructionCache.WaitForSeconds(0.15f);
        sr.color = new Color(1, 1, 1, 0);
        yield return YieldlnstructionCache.WaitForSeconds(0.15f);
        sr.color = new Color(1, 1, 1, 1);
        yield return YieldlnstructionCache.WaitForSeconds(0.15f);
        sr.color = new Color(1, 1, 1, 1);
        player.isHit = true;
        yield return null;
    }

    public IEnumerator AttackWait(Player.AttackMode attackMode)
    {
        yield return YieldlnstructionCache.WaitForSeconds(0.5f);
        if (player._currentAttack == attackMode)
        {
            player._currentAttack = Player.AttackMode.None;
            player.isCombo = false;
        }
    }

    public IEnumerator Save()
    {
        saveMessage.SetActive(true);
        yield return YieldlnstructionCache.WaitForSeconds(1);
        saveMessage.SetActive(false);
    }
}
