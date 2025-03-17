using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScripts : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //looking the cursor to the middle of the screen and making it invisible
    }

    void Update()
    {
        //getting mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //rotation around the x axis (look up and down)
        xRotation -= mouseY;

        //clamp the rotation
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);
        
        //rotation around the y axis (look left and right)
        yRotation += mouseX;

        //Apply rotation to our tranform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
