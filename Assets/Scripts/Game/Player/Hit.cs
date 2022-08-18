using System.Collections;
using Game.Player;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public GameObject saveMessage;
    private Player _player;
    private SpriteRenderer _sr;
    
    void Start()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _player = GetComponent<Player>();
    }

    public IEnumerator HitAni()
    {
        Debug.Log("맞음");
        _player.isHit = false;
        _sr.color = new Color(1, 1, 1, 0);
        yield return YieldInstructionCache.WaitForSeconds(0.15f);
        _sr.color = new Color(1, 1, 1, 1);
        yield return YieldInstructionCache.WaitForSeconds(0.15f);
        _sr.color = new Color(1, 1, 1, 0);
        yield return YieldInstructionCache.WaitForSeconds(0.15f);
        _sr.color = new Color(1, 1, 1, 1);
        yield return YieldInstructionCache.WaitForSeconds(0.15f);
        _sr.color = new Color(1, 1, 1, 1);
        _player.isHit = true;
        yield return null;
    }

    public IEnumerator AttackWait(Player.AttackMode attackMode)
    {
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        if (_player.currentAttack == attackMode)
        {
            _player.currentAttack = Player.AttackMode.None;
            _player.isCombo = false;
        }
    }

    public IEnumerator Save()
    {
        saveMessage.SetActive(true);
        yield return YieldInstructionCache.WaitForSeconds(1);
        saveMessage.SetActive(false);
    }
}
