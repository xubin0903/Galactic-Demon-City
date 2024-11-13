using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvents : MonoBehaviour
{
   private Player player;
    private void Awake()
    {
        player=GetComponentInParent<Player>();
    }
    private void AnimatorEventnTrigger()
    {
        player.AttackOver();
    }
}
