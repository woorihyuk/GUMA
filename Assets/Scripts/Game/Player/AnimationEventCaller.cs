using Game.Player;
using UnityEngine;

public class AnimationEventCaller : MonoBehaviour
{
    public Player player;

    public void Attack1Fx()
    {
        player.OnAnimationAttackFx(Player.AttackMode.First);
    }
    
    public void Attack2Fx()
    {
        player.OnAnimationAttackFx(Player.AttackMode.Second);
    }
    
    public void Attack3Fx()
    {
        player.OnAnimationAttackFx(Player.AttackMode.Third);
    }

    public void ShootFx()
    {
        player.OnAnimationAttackFx(Player.AttackMode.FirstShoot);
    }

    public void Attack1End()
    {
        player.OnAnimationAttackEnd(Player.AttackMode.First);
    }

    public void Attack2End()
    {
        player.OnAnimationAttackEnd(Player.AttackMode.Second);
    }

    public void Attack3End()
    {
        player.OnAnimationAttackEnd(Player.AttackMode.Third);
    }
    
    public void OnBackStepEnd()
    {
        player.OnAnimationBackStepEnd();
    }
    
    public void OnDashEnd()
    {
        player.OnAnimationDashEnd();
    }

    public void OnDieEnd()
    {
        player.IsDie();
    }
    public void ShootEnd()
    {
        player.OnAnimationAttackEnd(Player.AttackMode.FirstShoot);
    }
    public void Shoot2End()
    {
        player.OnAnimationAttackEnd(Player.AttackMode.SecondShoot);
    }
}
