using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill : Skill
{
    [Header("Sword Skill Settings")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private float swordSpeed;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float gravity;
    private Vector2 finalDir;

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
    }
    public override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir=new Vector2(AimDir().normalized.x*launchDir.x,AimDir().normalized.y*launchDir.y);
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

    }
    public void CreateSword()
    {
        var newsword = Instantiate(swordPrefab, PlayerManager.instance.player.transform.position,player.transform.rotation);
        var swordController=newsword.GetComponent<Sword_Skill_Controller>();
        //检查是否已经有其他的sword
        player.CheckSword(newsword);
        
        swordController.SetupSword(finalDir,gravity);
        swordController.AnimationSword(true);
        SetActiveDots(false);//扔出关闭锚点
    }
    public Vector2 AimDir()
    {
        var  playerPos = PlayerManager.instance.player.transform.position;
        var mounsePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var dir=mounsePos-playerPos;
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
            AimDir().normalized.y * launchDir.y) *t+ 0.5f * (Physics2D.gravity * gravity * t * t);
        return Position;
    }
    public void SetActiveDots(bool _isActive)
    {
        for (int i = 0; i < dotNumber; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }
}
