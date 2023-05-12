using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header("References")]
    private InputActions inputActions;

    [Header("Movement Settings")]
    public bool analogMovement = false;

    #region -Singleton Declaration-
    private static InputHandler _instance;

    public static InputHandler instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("Cannot find a reference of InputHandler in scene, Try to attach the script to a gameObject");
            }
            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        inputActions = new InputActions();
        _instance = this;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

 
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public Vector2 GetMovementVector()
    {
        Vector2 moveVec = inputActions.PlayerInputs.Move.ReadValue<Vector2>();
        return moveVec;
    }

    public Vector2 GetMouseLook()
    {
        Vector2 mouseLookVec = inputActions.PlayerInputs.MouseLook.ReadValue<Vector2>();
        return mouseLookVec;
    }
    public bool GetFireButton()
    {
        bool isFire = false;
        if (inputActions.PlayerInputs.Fire.IsPressed())
        {
            isFire = true;
        }
        return isFire;
    } 
    
    public bool GetSprintPress()
    {
        bool isSprint = false;
        if (inputActions.PlayerInputs.Sprint.IsPressed())
        {
            isSprint = true;
        }
        return isSprint;
    } 
    
    public bool GetJumpPress()
    {
        bool isJump = false;
        if (inputActions.PlayerInputs.Jump.IsPressed())
        {
            isJump = true;
        }
        return isJump;
    }

    public bool GetCrouchPress()
    {
        bool isCrouch = false;
        if (inputActions.PlayerInputs.Crouch.IsPressed())
        {
            isCrouch = true;
        }
        return isCrouch;
    }
}
