using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform gunNuzzle;
    private CharacterController playerCharController;
    private PuzzleManager puzzleManager;

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
        puzzleManager = GameObject.Find(GameTags.puzzleMan).GetComponent<PuzzleManager>();
    }

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



    private void FireWeapon()
    {
        if (InputHandler.instance.GetFireButton() && Time.time >= nextFireTime)
        {
            RaycastHit hit;
            if (Physics.Raycast(gunNuzzle.position, transform.forward, out hit))
            {
                Debug.Log("Shooot");
                if (hit.collider != null)
                {
                    Debug.Log("Hit Something");
                }
                if (hit.collider.CompareTag(GameTags.sphereTag))
                {
                    // Get the color of the sphere and check if it matches the color variable
                    Renderer sphereRenderer = hit.collider.GetComponent<Renderer>();
                    if (sphereRenderer != null && sphereRenderer.material.color == puzzleManager.GetCurrentColorToShoot())
                    {
                        // Destroy the sphere if the colors match
                        Destroy(hit.collider.gameObject);
                        RemoveShotColor();
                    }
                }
            }
            nextFireTime = Time.time + fireRate;
        }
    }

    private void RemoveShotColor()
    {
        if (puzzleManager.correctColorOrder.Count != 0)
        {
            //removes the first color from the list, if the player shoots the right color
            puzzleManager.correctColorOrder.RemoveAt(0);
        }
    }

}
