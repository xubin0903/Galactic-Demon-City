using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen_UI : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
       animator=GetComponent<Animator>();
        ResetAnimator();
    }
    public void FadeIn()
    {
        ResetAnimator();
        animator.GetComponent<Animator>().SetBool("FadeOut", false);
        animator.GetComponent<Animator>().SetBool("FadeIn", true);
    }
    public void FadeOut()
    {
        ResetAnimator();
        animator.GetComponent<Animator>().SetBool("FadeIn", false);
        animator.GetComponent<Animator>().SetBool("FadeOut", true);
    }
    public void ResetAnimator()
    {
        animator.SetBool("FadeIn", false);
        animator.SetBool("FadeOut", false);
    }
}
