using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharController : MonoBehaviour
{
    [Header("References")]
    private InputHandler playerInputAction;
    private CharacterController charController;
    private GameObject mainCamera;
    private PlayerAnimController animController;

    [Header("Variables")]
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 5.5f;
    [SerializeField] private float crouchSpeed = 1.5f;
    private float speedChangeRate = 10.0f; //for acceleration and decelaration
    private float rotationSmoothTime = 0.12f;
    private float speed;
    private float animationBlend;
    private float targetRotation = 0.0f;
    private float rotationVelocity;
    private float verticalVelocity;
    //jump
    public bool isGrounded;
    private float defaultJumpTimeout = 0.50f;
    private float jumpTimeOut;
    private float jumpHeight = 1.2f;
    private float gravity = -15.0f;
    private float terminalVelocity = 53.0f;
    private float minYPos = -0.75f;


    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        animController = GetComponent<PlayerAnimController>();
        //animator = GetComponent<Animator>();
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag(GameTags.mainCam);
        }
    }

    void Start()
    {
        playerInputAction = InputHandler.instance;

        //reset values on Start
        jumpTimeOut = defaultJumpTimeout;
    }

    void Update()
    {
        //call functions
        Move();
        CheckGrounded();
        JumpAndGravity();
        FireWeapon();
    }

    private void Move()
    {
        // set target speed based on movement
        float targetSpeed = playerInputAction.GetSprintPress() ? sprintSpeed : moveSpeed;
        if (playerInputAction.GetCrouchPress())
        {
            targetSpeed = crouchSpeed;
        }
        // if there is no input, set the target speed to 0
        if (playerInputAction.GetMovementVector() == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(charController.velocity.x, 0.0f, charController.velocity.z).magnitude;


        float speedOffset = 0.1f;
        float inputMagnitude = playerInputAction.analogMovement ? playerInputAction.GetMovementVector().magnitude : 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * speedChangeRate);

            // round speed to 3 decimal places
            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = targetSpeed;
        }

        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(playerInputAction.GetMovementVector().x, 0.0f, playerInputAction.GetMovementVector().y).normalized;
        if (playerInputAction.GetMovementVector() != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                rotationSmoothTime);

            // rotate to face playerInputAction direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        // move the player
        charController.Move(targetDirection.normalized * (speed * Time.deltaTime) +
                         new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

        // update animator 
        animController.SetMovementAnim(animationBlend);
        animController.SetCrouchAnim(playerInputAction.GetCrouchPress());

    }

    private void CheckGrounded()
    {
        isGrounded = charController.isGrounded;
        animController.SetGroundedBool(isGrounded);
    }
    
    private void JumpAndGravity()
    {
        if (isGrounded)
        {
            animController.SetJumpBool(false);
            // stop our velocity dropping infinitely when grounded
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = 0.0f; // -2f;
            }

            // Jump
            if (playerInputAction.GetJumpPress() && jumpTimeOut <= 0.0f)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                animController.SetJumpBool(true);
            }

            // jump timeout
            if (jumpTimeOut >= 0.0f)
            {
                jumpTimeOut -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            jumpTimeOut = defaultJumpTimeout;
        }

        // apply gravity over time 
        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void FireWeapon()
    {
        if (playerInputAction.GetFireButton() )
        {
            animController.SetFireTrigger();
            Debug.Log("Shooting bullets");
        }
    }
}
