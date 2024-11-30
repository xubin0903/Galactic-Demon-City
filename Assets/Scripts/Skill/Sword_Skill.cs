using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum SkillType 
{
    Regular,
    Bounce,
    Pierce,
    Spin,
}
public class Sword_Skill : Skill
{
    [Header("Sword Skill Settings")]
    private bool isFlip = true;
    [SerializeField] public bool canThrow;
    public bool doublePierce;
    [SerializeField] private float gravity;
    private Vector2 finalDir;
    private bool haveDots;
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private SkillType skillType;
    public SkillSlot_UI RegularUnlockSlot;
    public SkillSlot_UI BounceUnlockSlot;
    public SkillSlot_UI PierceUnlockSlot;
    public SkillSlot_UI DoublePierceUnlockSlot;
    public SkillSlot_UI amountDoubleUnlockSlot;

    [Header("Bouncing Info")]
    [SerializeField] private int maxBounceNumber;
    [SerializeField] private bool isBouncing;
    [SerializeField] private float bounceSpeed;
    [SerializeField] private float bounceGravity;
    [SerializeField] private bool isFlipBounce;
    [SerializeField] private bool haveDotsBounce;
    [Header("Pierce Info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceSpeed;
    [SerializeField] private Vector2 pierceDir;
    [SerializeField] private float pierceGravity;
    [SerializeField] private bool isFlipPierce;
    [SerializeField] private bool haveDotsPierce;

    [Header("Dots Info")]
    [SerializeField] private int dotNumber;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotParent;


    private GameObject[] dots;
    protected override void Start()
    {
        base.Start();
        CreateDots();
        BounceUnlockSlot.GetComponent<Button>().onClick.AddListener(() =>BouceSkillUnlock());
        PierceUnlockSlot.GetComponent<Button>().onClick.AddListener(() => PierceSkillUnlock());
        RegularUnlockSlot.GetComponent<Button>().onClick.AddListener(() => ReguarSkillUnlock());
        DoublePierceUnlockSlot.GetComponent<Button>().onClick.AddListener(() => DoubePierceUnlock());
        amountDoubleUnlockSlot.GetComponent<Button>().onClick.AddListener(() => AmountDoubleUnlock());

    }
    public override bool CanUseSkill()
    {
        if (!canThrow)
        {
            return false;
        }
        return base.CanUseSkill();
    }
    public override void UseSkill()
    {
        //在player收剑时进入冷却在player脚本中实现
    }
    public override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDir().normalized.x * launchDir.x, AimDir().normalized.y * launchDir.y);
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dotNumber; i++)
            {
                var dotPos = DotsPosition(i * spaceBetweenDots);
                dots[i].transform.position = dotPos;

            }
        }
        if (dots == null)
        {
            Debug.LogError("dots array is null! Ensure CreateDots is called in Start.");
        }
        SetSword();

    }
    public void CreateSword()
    {
        var newsword = Instantiate(swordPrefab, PlayerManager.instance.player.transform.position, player.transform.rotation);
        var swordController = newsword.GetComponent<Sword_Skill_Controller>();
        //检查是否已经有其他的sword
        player.CheckSword(newsword);
        //根据类型设置sword的属性
        if (skillType == SkillType.Bounce)
        {
            newsword.GetComponent<Sword_Skill_Controller>().SetBounce(true, maxBounceNumber, bounceSpeed);
        }
        else if (skillType == SkillType.Pierce)
        {
            newsword.GetComponent<Sword_Skill_Controller>().SetPierce(true, pierceAmount, pierceSpeed);
        }


        swordController.SetupSword(finalDir, gravity, damage);
        swordController.AnimationSword(isFlip);
        SetActiveDots(false);//扔出关闭锚点
    }
    public Vector2 AimDir()
    {
        var playerPos = PlayerManager.instance.player.transform.position;
        var mounsePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var dir = mounsePos - playerPos;
        return dir;
    }
    private void CreateDots()
    {
        if (dots != null)
        {
            return;
        }
        dots = new GameObject[dotNumber];
        for (int i = 0; i < dotNumber; i++)
        {
            //Debug.Log("Dots Created");
            //if(dotPrefab == null)
            //{
            //    Debug.LogError("DotPrefab is null!");
            //}
            //if(dotParent == null)
            //{
            //    Debug.LogError("DotParent is null!");
            //}
            //if(player == null)
            //{
            //    Debug.LogError("Player is null!");
            //}
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotParent);
            dots[i].SetActive(false);
        }

    }
    //以时间间隔t计算出每一个点的位置
    public Vector2 DotsPosition(float t)
    {
        var Position = (Vector2)(player.transform.position) + new Vector2(AimDir().normalized.x * launchDir.x,
            AimDir().normalized.y * launchDir.y) * t + 0.5f * (Physics2D.gravity * gravity * t * t);
        return Position;
    }
    public void SetActiveDots(bool _isActive)
    {
        //笔直发射的sword不需要显示锚点
        if (skillType == SkillType.Pierce)
        {
            return;
        }
        for (int i = 0; i < dotNumber; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }
    //根据类型设置sword的属性
    private void SetSword()
    {
        if (skillType == SkillType.Bounce)
        {
            gravity = bounceGravity;
            isFlip = isFlipBounce;
            haveDots = haveDotsBounce;
        } else if (skillType == SkillType.Pierce)
        {
            launchDir = pierceDir;
            gravity = pierceGravity;
            isFlip = isFlipPierce;
            haveDots = haveDotsPierce;
        }
    }
    private void ReguarSkillUnlock()
    {
        if (RegularUnlockSlot.unlocked)
        {
            skillType = SkillType.Regular;
            canThrow = true;
        }
    }
    private void BouceSkillUnlock()
    {
        if (BounceUnlockSlot.unlocked)
        {
            skillType = SkillType.Bounce;
        }
    }
    private void PierceSkillUnlock()
    {
        if (PierceUnlockSlot.unlocked)
        {
            skillType = SkillType.Pierce;
        }
    }
    private void DoubePierceUnlock()
    {
        if(DoublePierceUnlockSlot.unlocked){
            doublePierce = true;
        }
    }
    private void AmountDoubleUnlock()
    {
        if(amountDoubleUnlockSlot.unlocked){
            pierceAmount *= 2;
            maxBounceNumber *= 2;
        }
    }
}