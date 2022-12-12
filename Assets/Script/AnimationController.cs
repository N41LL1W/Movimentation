using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AnimationStates
{
    WALK,
    RUN,
    JUMP,
    ATTACK01,
    ATTACK02,
    ATTACK03,
    IDDLE
}

public class AnimationController : MonoBehaviour
{
    [Header("Animations")]
    //Animations
    private Animator animator;
    public float vertical;
    public float horizontal;
    
    public static AnimationController Instance;

    void Start()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        this.vertical = Input.GetAxis("Vertical");
        this.horizontal = Input.GetAxis("Horizontal");
        
        this.animator.SetFloat("Vertical", vertical);
        this.animator.SetFloat("Horizontal", horizontal);
        
        if (Input.GetKey(KeyCode.LeftShift) == true || Input.GetKey(KeyCode.RightShift) == true)
        {
            AnimationController.Instance.PlayAnimation(AnimationStates.RUN);
        }
        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            AnimationController.Instance.PlayAnimation(AnimationStates.IDDLE);
        }
        else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            AnimationController.Instance.PlayAnimation(AnimationStates.WALK);
        }
        else if (Input.GetAxis("Jump") == 0) 
        {
            AnimationController.Instance.PlayAnimation(AnimationStates.JUMP);
        }
    }

    public void PlayAnimation(AnimationStates stateAnimation)
    {
        switch (stateAnimation)
        {
            case AnimationStates.IDDLE:
            {
                StopAnimations();
                animator.SetBool("inIddle", true);
            }
                break;
            case AnimationStates.WALK:
            {
                StopAnimations();
                animator.SetBool("inWalk", true);
            }
                break;
            case AnimationStates.RUN:
            {
                StopAnimations();
                animator.SetBool("inRun", true);
            }
                break;
            case AnimationStates.JUMP:
            {
                StopAnimations();
                animator.SetTrigger("inJump");
            }
                break;
            case AnimationStates.ATTACK01:
            {
                StopAnimations();
                animator.SetTrigger("Attack01");
            }
                break;
            case AnimationStates.ATTACK02:
            {
                StopAnimations();
                animator.SetTrigger("Attack02");
            }
                break;
            case AnimationStates.ATTACK03:
            {
                StopAnimations();
                animator.SetTrigger("Attack03");
            }
                break;
        }
    }

    public void StopAnimations()
    {
        animator.SetBool("inIddle", false);
        animator.SetBool("inWalk", false);
        animator.SetBool("inRun", false);
    }
}