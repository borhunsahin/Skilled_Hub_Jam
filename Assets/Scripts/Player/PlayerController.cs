using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerAnimationController playerAnimController;

    public Vector2 input;
    private Vector3 direction;

    [SerializeField] public Player player;
    [SerializeField] public Gravity gravity;

    private Camera playerCam;

    [HideInInspector] public float currentSpeedMultiplier;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimController = GameObject.Find("Player").GetComponentInChildren<PlayerAnimationController>();
        playerCam = Camera.main;

        currentSpeedMultiplier = 1;
    }
    void Update()
    {

        FreeMode();
        GravityApply();
        Movement();

    }
    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0, input.y); 
    }
    private void Movement()
    {
        characterController.Move((new Vector3(direction.x * currentSpeedMultiplier, direction.y, direction.z * currentSpeedMultiplier) * player.speed  * Time.deltaTime));
    }
    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.started && player.isFreeMode)
            currentSpeedMultiplier = player.speedMultiplier;
        //else if(context.started && player.isActionMode && !player.isCrouch)
            //currentSpeedMultiplier = player.speedMultiplier;
        else if (context.canceled)
            currentSpeedMultiplier = 1;
    }
    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        Debug.Log("Interact F");
    }
    private void FreeMode()
    {
        if (input.sqrMagnitude > 0.05)
        {
            direction = Quaternion.Euler(0.0f, playerCam.transform.eulerAngles.y, 0.0f) * new Vector3(input.x, 0.0f, input.y);
            var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500f * Time.deltaTime);
        }
    }
    private void GravityApply()
    {
        if (characterController.isGrounded && gravity.velocity < -2)
        {
            gravity.velocity = -1f;
        }
        else
        {
            gravity.velocity += gravity.gravity * gravity.gravityMultipler * Time.deltaTime;
            direction.y = gravity.velocity;
        }
    }
}
[Serializable]
public struct Player
{
    public float speed;
    public float speedMultiplier;

    public bool isFreeMode;
}
[Serializable]
public struct Gravity
{
    public float gravity;
    public float gravityMultipler;
    [HideInInspector] public float velocity;
}
