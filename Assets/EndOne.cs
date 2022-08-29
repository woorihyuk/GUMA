using Game;
using Game.State;
using UnityEngine;

public class EndOne : MonoBehaviour
{
    private void Start()
    {
        TextManager.Instance.OnInput("End_0");
    }

    private void Update()
    {
        GameUIManager.Instance.SetActivePlayerHud(false);
        
        if (Input.GetKeyDown(KeyCode.E) && !GameUIManager.Instance.pauseGroup.gameObject.activeSelf)
        {
            if (StateManager.Instance.currentState == StateType.Talking)
            {
                TextManager.Instance.OnInputWithLast();
            }
        }
    }
}
