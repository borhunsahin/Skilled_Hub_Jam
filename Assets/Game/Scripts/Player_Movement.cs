using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Player_Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody rb;
    public Transform cameraTransform;
    private bool isMoving;
    public Animator animator;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
 

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        animator.SetBool("isMoving", isMoving);

        if (horizontalInput != 0 || verticalInput != 0)
        {
            Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

            // Kameraya göre hareket yönünü hesapla
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;
            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;
            moveDirection.Normalize();

            // Hareket ettir
            Vector3 movement = moveDirection * moveSpeed * Time.deltaTime;
            rb.MovePosition(transform.position + movement);

            // Yönü ayarla (sürekli dönme yok, sadece input yönüne doðru bak)
            if (moveDirection != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);
            }
        }
        if (horizontalInput == 0 && verticalInput == 0) 
        { 
            isMoving = false; 
            
        }
        if (horizontalInput != 0 || verticalInput != 0)
        {
            isMoving = true;
        }


    }
    
   
}


