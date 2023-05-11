using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float mouseSensitivity = 2.0f;
    private float mouseClampAngle = 90.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //call functions
        MoveCamera();
    }

    private void MoveCamera()
    {
        Vector2 mouseDelta = InputHandler.instance.GetMouseLook();
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x -= mouseY;
        rotation.x = Mathf.Clamp(rotation.x, -mouseClampAngle, mouseClampAngle);
        transform.rotation = Quaternion.Euler(rotation);
    }
}
