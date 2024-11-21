using UnityEngine;

public class StateManager : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        AdjustingRodStrength,
        Fishing,
        Moving,
    }

    public static PlayerState CurrPlayerState {get; private set;}
    public static PlayerState[] ValidStatesForMoving {get; private set;}
    
    void Start()
    {
        CurrPlayerState = PlayerState.Idle;
        ValidStatesForMoving = new[] { PlayerState.Idle, PlayerState.Moving };
    }

    public static void ChangePlayerState(PlayerState state)
    {
        CurrPlayerState = state;
    }
}
