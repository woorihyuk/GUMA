using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;
    
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public StateType currentState = StateType.None;
}