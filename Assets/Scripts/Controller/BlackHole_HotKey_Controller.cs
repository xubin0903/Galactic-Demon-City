using TMPro;
using UnityEngine;

public class BlackHole_KeyCode_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode key;
    private TextMeshProUGUI text;
    private Transform enemyTransform;
    private BlackHole_Skill_Controller balckHole_Skill;
    private bool isToAdded;
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        sr = GetComponent<SpriteRenderer>();
    }
    public void SetKeyCode(KeyCode _key,Transform _EnemyTransform,BlackHole_Skill_Controller _balckHole_Skill)
    {
        enemyTransform = _EnemyTransform;

        balckHole_Skill = _balckHole_Skill;
        key = _key;
        text.text = key.ToString();

    }
    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            if(isToAdded == true)
            {
                return;
            }
            isToAdded = true;
            balckHole_Skill.AddEnemyToTargets(enemyTransform);
            text.color = Color.clear;
            sr.color = Color.clear;
            //Debug.Log("Key Down"+key.ToString());
        }
    }

}
