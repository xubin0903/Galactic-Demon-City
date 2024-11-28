using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "IceandFire_Effect", menuName = "Data/ItemEffect/IceandFire_Effect", order = 2)]
public class IceandFire_Effect : ItemEffect
{
  public GameObject iceandFireEffect;
  public Vector2 velocity;
    public override void ExecuteEffect(Transform target)
    {
        Debug.Log("IceandFire_Effect");
         Player player = PlayerManager.instance.player;
        if (player.comobatCount != 2)
        {
            Debug.Log("comobatCount != 2");
            return;
        }
        if (player == null)
        {
            Debug.Log("Player is null");
            return;
        }
        GameObject effect = Instantiate(iceandFireEffect,player.transform.position,player.transform.rotation );
        effect.GetComponent<Rigidbody2D>().velocity = player.faceDir*velocity;
        Destroy(effect, 10f);

    }
}
