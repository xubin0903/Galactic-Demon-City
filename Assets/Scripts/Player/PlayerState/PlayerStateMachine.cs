using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine 
{
    public Player player;
    public PlayerState currentState;
    // Start is called before the first frame update
    public void Initialize(PlayerState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    // Update is called once per frame
   public void ChangeState(PlayerState newState)
    {
        if (currentState!= null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }   
}
