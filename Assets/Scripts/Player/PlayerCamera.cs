using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    PlayerController playerController;

    [SerializeField] private GameObject player;

    [SerializeField] private _Camera _camera;
    private Rotation rotation;
    private Vector2 input;

    private float zoomInput;

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    private void Update()
    {
        CameraMovement();
    }
    void LateUpdate()
    {
        CameraRotate(player);
    }
    public void Look(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }
    public void Scroll(InputAction.CallbackContext context)
    {
        zoomInput = context.ReadValue<float>();
    }
    private void CameraMovement()
    {
        rotation.x += input.x * _camera.sensitivityX * Time.deltaTime;
        rotation.y += input.y * _camera.sensitivityY * Time.deltaTime;
        rotation.y = Mathf.Clamp(rotation.y, _camera.minYAngleValue, _camera.maxYAngleValue);

        _camera.zoomValue += -zoomInput / 120 * _camera.zoomMultiplier;
        _camera.zoomValue = Mathf.Clamp(_camera.zoomValue, _camera.minZoomValue, _camera.maxZoomValue);
    }
    private void CameraRotate(GameObject gameObject)
    {
        transform.eulerAngles = new Vector3(rotation.y, rotation.x, 0.0f);
        transform.position = gameObject.transform.position - transform.forward * Mathf.Clamp(_camera.zoomValue, 3f, 10f);
    }
}
[Serializable]
public struct _Camera
{
    public float minYAngleValue, maxYAngleValue;
    public float sensitivityX;
    public float sensitivityY;

    public float minZoomValue, maxZoomValue;
    public float zoomValue;
    public float zoomMultiplier;
}
public struct Rotation
{
    public float x;
    public float y;
}
