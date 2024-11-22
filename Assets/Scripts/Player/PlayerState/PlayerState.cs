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
    protected float stateTimer;
   public PlayerState(Player _player, PlayerStateMachine _stateMachine,string _animName)
   {
      this.player = _player;
      this.stateMachine = _stateMachine;
      this.animName = _animName;
      this.rb = player.GetComponent<Rigidbody2D>();
   }

   public virtual void Enter()
   {
      Debug.Log(animName + ": Enter");
      player.animator.SetBool(animName,true);
     
   }


   public virtual void Update()
   {
      stateTimer -= Time.deltaTime;
   }
   public virtual void Exit()
   {
     Debug.Log(animName + ": Exit");
      player.animator.SetBool(animName,false);

   }
}
