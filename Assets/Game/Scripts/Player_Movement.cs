using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class Player_Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody rb;
    public Transform cameraTransform;
    private bool isMoving;
    public Animator animator;
    public GameObject Floor2;
    [Header("UI")]
    [Range(0, 100)] public float thirstyAmount;
    [Range(0, 100)] public float hungerAmount;
    public float negSpeed = 5;
    public Slider foodSlider;
    public Slider ThirstySlider;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        hungerAmount = 100;
        thirstyAmount = 100;
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

    private void Update()
    {
        thirstyAmount = thirstyAmount - Time.deltaTime * negSpeed;
        hungerAmount = hungerAmount - Time.deltaTime * negSpeed;

        foodSlider.value = hungerAmount;
        ThirstySlider.value = thirstyAmount;
    }
    private void OnTriggerStay(Collider other)
    {
       
        if (other.gameObject.CompareTag("Light") && Input.GetKeyDown(KeyCode.E))
        {
            Light lightComponent = other.gameObject.GetComponentInChildren<Light>();
            if (lightComponent != null)
            {
                lightComponent.enabled = !lightComponent.enabled;
            }
        }
        if (other.gameObject.CompareTag("Water") && Input.GetKeyDown(KeyCode.E))
        {
            Destroy(other.gameObject);
            thirstyAmount += 10;
        }
        if (other.gameObject.CompareTag("Food") && Input.GetKeyDown(KeyCode.E))
        {
            Destroy(other.gameObject);
            hungerAmount += 10;
        }

    }


}


