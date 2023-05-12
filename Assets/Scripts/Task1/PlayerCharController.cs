using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharController : MonoBehaviour
{
    [Header("References")]
    private InputHandler input;
    private CharacterController controller;
    private GameObject mainCamera;
    private Animator animator;

    [Header("Variables")]
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 5.5f;
    private float speedChangeRate = 10.0f; //for acceleration and decelaration
    private float rotationSmoothTime = 0.12f;
    private float speed;
    private float animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag(GameTags.mainCam);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        input = InputHandler.instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Move()
    {
        // set target speed based on movement
        float targetSpeed = input.GetSprintPress() ? sprintSpeed : moveSpeed;

        // if there is no input, set the target speed to 0
        if (input.GetMovementVector() == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;


        float speedOffset = 0.1f;
        float inputMagnitude = input.analogMovement ? input.GetMovementVector().magnitude : 1f;

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
        Vector3 inputDirection = new Vector3(input.GetMovementVector().x, 0.0f, input.GetMovementVector().y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (input.GetMovementVector() != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                rotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move the player
        controller.Move(targetDirection.normalized * (speed * Time.deltaTime) +
                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        // update animator 
        animator.SetFloat(AnimTags.speed, animationBlend);
        //animator.SetFloat(_animIDMotionSpeed, inputMagnitude);

    }
}
