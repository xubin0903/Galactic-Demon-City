using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerState
{
   protected Player player;
   protected PlayerStateMachine stateMachine;
   protected Rigidbody2D rb;
   protected string animName;
   public PlayerState(Player _player, PlayerStateMachine _stateMachine,string _animName)
   {
      this.player = _player;
      this.stateMachine = _stateMachine;
      this.animName = _animName;
   }

   public virtual void Enter()
   {
      Debug.Log("PlayerState: Enter");
     
   }


   public virtual void Update()
   {
      Debug.Log("PlayerState: Update");
   }
   public virtual void Exit()
   {
      Debug.Log("PlayerState: Exit");
   }
}
