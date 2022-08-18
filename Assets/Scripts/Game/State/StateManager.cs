namespace Game.State
{
    public class StateManager : Singleton<StateManager>
    {
        public StateType currentState = StateType.None;
    }
}