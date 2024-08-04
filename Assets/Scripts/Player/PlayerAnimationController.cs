using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public PlayerController playerController;
    public Animator animator;
   
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {      
        animator.SetFloat("inputX",playerController.input.x * playerController.currentSpeedMultiplier);
        animator.SetFloat("inputY", playerController.input.y * playerController.currentSpeedMultiplier);
        animator.SetBool("isAlive", playerController.isAlive);

        
    }
}
