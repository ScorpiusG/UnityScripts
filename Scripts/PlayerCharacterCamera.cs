/*
 *  Camera pivot script.
 *  Allows the player to move the camera with the mouse or the controller's analog stick (usually the right one).\
 *  
 *  Attach this onto the PIVOT of the camera, NOT ON THE CAMERA ITSELF.
 *  
 *  Hierarchy example:
 *  - CameraPivot (empty GameObject)  <== attach to this one
 *  -     MainCamera (has Camera/CinemachineVCam component)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterCamera : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 5f;
    [SerializeField] private InputActionReference inputCamera;
    private bool isInputCameraPresent = false;
    private Vector3 cameraDirection = Vector3.zero;

    private void Awake()
    {
        cameraDirection = Vector3.zero;
        CheckForComponents();
    }

    // If components were added or removed during runtime (most likely by other scripts), call this again manually.
    public void CheckForComponents()
    {
        isInputCameraPresent = inputCamera != null;
    }

    // Enable input.
    private void OnEnable()
    {
        cameraDirection = Vector3.zero;
        if (!isInputCameraPresent) return;
        inputCamera.action.Enable();
    }
    // Disable input.
    private void OnDisable()
    {
        cameraDirection = Vector3.zero;
        if (!isInputCameraPresent) return;
        inputCamera.action.Disable();
    }

    private void Update()
    {
        if (!isInputCameraPresent) return;
        cameraDirection = inputCamera.action.ReadValue<Vector2>();
        transform.Rotate(cameraDirection.x * cameraSpeed * Time.deltaTime * Vector3.up);
    }
}
