using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerAnimationController playerAnimController;

    public Vector2 input;
    private Vector3 direction;

    [SerializeField] public Player player;
    [SerializeField] public Gravity gravity;

    private Camera playerCam;
    [SerializeField] private GameObject secondFloor;

    [HideInInspector] public float currentSpeedMultiplier;

    [Range(0, 100)] public float thirstyAmount;
    [Range(0, 100)] public float hungerAmount;
    [Range(0, 100)] public float stressAmount;
    public float negSpeed = 5;
    public Slider foodSlider;
    public Slider ThirstySlider;
    public Slider StressSlider;
    public TextMeshProUGUI InteractionName;
    public GameObject InteractionPanel;

    public GameObject MedicineIcon;

    public bool isAlive = true;



    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimController = GameObject.Find("Player").GetComponentInChildren<PlayerAnimationController>();
        playerCam = Camera.main;

        currentSpeedMultiplier = 1;

        hungerAmount = 100;
        thirstyAmount = 100;
        stressAmount = 80;

       
    }
    void Update()
    {
        if (gameObject.transform.position.y >= 3.50)
            secondFloor.SetActive(true);
        else
            secondFloor.SetActive(false);

        FreeMode();
        GravityApply();
        Movement();

        thirstyAmount = thirstyAmount - Time.deltaTime * negSpeed;
        hungerAmount = hungerAmount - Time.deltaTime * negSpeed;

        foodSlider.value = hungerAmount;

        ThirstySlider.value = thirstyAmount; 

        StressSlider.value = stressAmount;

        isAlive = false;

        if (hungerAmount <= 0 || thirstyAmount <= 0 ||stressAmount <= 0)
        {
            Debug.Log("Dead1");
            isAlive = false;
        }


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

        Debug.Log("F");
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Light"))
        {
            InteractionUI(other.gameObject.name);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Light lightComponent = other.gameObject.GetComponentInChildren<Light>();
                if (lightComponent != null)
                {
                    lightComponent.enabled = !lightComponent.enabled;
                    InteractionPanel.SetActive(false);
                }
            }

        }
        if (other.gameObject.CompareTag("BrokenLight") && Input.GetKeyDown(KeyCode.E))
        {
            //Elekritik Çaprma Animasyonu (Bool ile kontrol ederiz )
            Debug.Log("dead2");
            isAlive = false;
        }

        if (other.gameObject.CompareTag("Water"))
        {
            InteractionUI(other.gameObject.name);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Destroy(other.gameObject);
                thirstyAmount += 10;
                InteractionPanel.SetActive(false);
            }

        }
        if (other.gameObject.CompareTag("Food"))
        {
            InteractionUI(other.gameObject.name);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Destroy(other.gameObject);
                hungerAmount += 10;
                InteractionPanel.SetActive(false);
            }

        }
        if (other.gameObject.CompareTag("Medicine"))
        {
            InteractionUI(other.gameObject.name);
            if (Input.GetKeyDown(KeyCode.E) && thirstyAmount <= 80)
            {
                isAlive = false;
                Debug.Log("dead3");
                Destroy(other.gameObject);

            }
            else if (Input.GetKeyDown(KeyCode.E) && thirstyAmount >= 80)
            {
                stressAmount -= 30;
                MedicineIcon.SetActive(false);
                Destroy(other.gameObject) ;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        InteractionPanel.SetActive(false);
    }
    private void InteractionUI(string Name)
    {
        InteractionPanel.SetActive(true);
        InteractionName.text = Name + " Use";
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
