using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController playerCharController;

    [Header("Variables")]
    //movement
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float mouseSensitivity = 3.0f;

    //weapon
    [SerializeField] private float fireRate = 1.0f;
    private float nextFireTime = 0.0f;
    private void Awake()
    {
        playerCharController = GetComponent<CharacterController>();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //call functions
        MovePlayer();
        FireWeapon();
    }

    private void MovePlayer()
    {
        Vector2 moveInput = InputHandler.instance.GetMovementVector();
        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        playerCharController.Move(moveDirection * moveSpeed * Time.deltaTime);

        //mouse movement
        Vector2 mouseDelta = InputHandler.instance.GetMouseLook();
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

    }

    //private void FireWeapon()
    //{
    //    bool isFireWeapon = InputHandler.instance.GetFireButton();
    //    if (isFireWeapon)
    //    {
    //        Debug.Log("FireWeapon");
    //    }


    //}

    private void FireWeapon()
    {
        if (InputHandler.instance.GetFireButton() && Time.time >= nextFireTime)
        {
            Debug.Log("FireWeapon");
            nextFireTime = Time.time + fireRate;
        }
    }

}
