using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAnimationTrigger : MonoBehaviour
{
    private Archer enemy;
    private void Awake()
    {
        enemy = GetComponentInParent<Archer>();
    }
    public void AttackFinish()
    {
        enemy.stateMachine.ChangeState(enemy.idleState);
        Debug.Log("Attack Finish");
    }
    public void AttackEvent()
    {
        GameObject newArrow=Instantiate(enemy.arrowPrefab,new Vector3(enemy.transform.position.x,enemy.transform.position.y+0.5f,enemy.transform.position.z),Quaternion.identity);
        newArrow.transform.localScale=enemy.transform.localScale;
        newArrow.GetComponent<Arrow_Controller>().SetUp(enemy.stats);

    }
    public void OpenCounterWindow() => enemy.OpenCounterWindow();
    public void CloseCounterWindow() => enemy.CloseCounterWindow();
    public void EnemyAfterDead()
    {
        StartCoroutine(EnemyDieAndDrop());
    }
    private IEnumerator EnemyDieAndDrop()
    {
        yield return new WaitForSeconds(1f);
        enemy.cd.enabled = false;
        enemy.rb.gravityScale = 10;//加快下落速度
                                   //掉落一定深度过后销毁敌人
        yield return new WaitForSeconds(10f);
        Destroy(enemy.gameObject);
    }
}
