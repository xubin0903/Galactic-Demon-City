using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bringer_Spell_Controller : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask playerLayer;
    private CharacterStats myStats;
    public void SetUp(CharacterStats stats)=> myStats = stats;
    private void AnimationTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, 0, playerLayer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Player> ()!= null)
            {
                collider.GetComponent<Player>().Damage(null);
                myStats.DoDamage(collider.GetComponent<CharacterStats>());
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(check.position, boxSize);
    }
    public void SetDestory()
    {
        Destroy(this.gameObject);
    }

}
