using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInputHandler : MonoBehaviour
{
    [Header("References")]
    private InputActions inputActions;

    private void Awake()
    {
        inputActions = new InputActions();
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
        inputActions.PlayerInputs.Fire.performed += e => ShootWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShootWeapon()
    {
        Debug.Log("Firriiiiiiiiiiii");
    }
}
