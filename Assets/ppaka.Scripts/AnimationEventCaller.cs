using UnityEngine;

public class AnimationEventCaller : MonoBehaviour
{
    public Player player;

    public void OnBackStepEnd()
    {
        player.OnAnimationBackStepEnd();
    }
    
    public void OnDashEnd()
    {
        player.OnAnimationDashEnd();
    }
}
