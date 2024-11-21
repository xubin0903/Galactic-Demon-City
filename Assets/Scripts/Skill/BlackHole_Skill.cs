using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill : Skill
{
    public GameObject blackHolePrefab;
    private GameObject blackHole;
    [Header("Skill Settings")]
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private bool canGrow;

    [SerializeField] private List<KeyCode> keyCodes;
    [SerializeField] private GameObject blackHoleKeyCodePrefab;
    [SerializeField] private bool canAttack;
    [SerializeField] private int maxAttackAmount;

    [SerializeField] private float attackCooldown;

    [SerializeField] private Vector3 CloneOffset;

    [SerializeField] private bool isShrinking;
    [SerializeField] private float shrinkSpeed;

    protected override void Start()
    {
        base.Start();
        
    }
    public override void Update()
    {
        base.Update();
    }
    public override void UseSkill()
    {
        base.UseSkill();
        CreateBlackHole();
    }
    public void CreateBlackHole()
    {
        blackHole = Instantiate(blackHolePrefab, PlayerManager.instance.player.transform.position , Quaternion.identity);
        blackHole.GetComponent<BlackHole_Skill_Controller>().SetupBlackHole(maxSize, growSpeed, canGrow, keyCodes, blackHoleKeyCodePrefab, canAttack, maxAttackAmount, attackCooldown, CloneOffset, isShrinking, shrinkSpeed);
    }
}
  
