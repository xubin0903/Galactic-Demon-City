using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    Transform sword;
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
       sword=player.sword.transform;
        if (sword.position.x > player.transform.position.x && player.faceDir == -1)
        {
            //·­×ª
            player.transform.localScale = new Vector3(-player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
        }
        else if (sword.position.x < player.transform.position.x && player.faceDir == 1)
        {
            //·­×ª
            player.transform.localScale = new Vector3(-player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
