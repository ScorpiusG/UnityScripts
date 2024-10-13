/*
 *  Character controller script, by ScorpiusG.
 *  Allows the player to move the character with any traditional inputs with a 2D vector.
 *  Movement after input is based on camera rotation. Check the script to see the logic or if adjustments are needed.
 *  
 *  Attach this onto the main Player object. In most cases, you should not attach this to a child object.
 *  
 *  Use EITHER a Rigidbody or CharacterController component and assign it to the respective variable.
 *  DO NOT USE BOTH COMPONENTS.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterMovement : MonoBehaviour
{
    [Header("Hover over an item to view its tooltip.")]
    [Header("Open this component's script for instructions.")]
    [Tooltip("Movement speed at maximum value. Adjust it accordingly, especially after switching components or logic involved (such as Rigidbody Setting).")]
    [SerializeField] private float movementSpeed = 5f;
    [Tooltip("Input used to handle movement. Said input should be Vector2 type.")]
    [SerializeField] private InputActionReference inputMovement;
    private bool isInputMovementPresent = false;
    [Tooltip("Assign the Rigidbody component here. If so, ignore the CharacterController and leave it null.")]
    [SerializeField] private Rigidbody mRigidbody;
    private bool isRigidbodyPresent = false;
    [Tooltip("If using a Rigidbody, sets how its movement is handled.")]
    [SerializeField] private RigidbodySetting rigidbodySetting;
    public enum RigidbodySetting
    {
        UseForce,
        SetVelocity
    }
    [Tooltip("Assign the CharacterController component here. If so, ignore the Rigidbody and leave it null.")]
    [SerializeField] private CharacterController mCharacterController;
    private bool isCharacterControllerPresent = false;
    private Vector2 movementDirection = Vector3.zero;
    private Vector3 characterDirection = Vector3.zero;
    [Tooltip("Add anti-softlock system if player falls off the map by repositioning them.")]
    [SerializeField] private bool resetPositionIfUnderMap = true;
    [Tooltip("Determine reposition location in world space.")]
    [SerializeField] private Vector3 resetPosition = Vector3.zero;
    [Tooltip("The Y-position the player must reach to activate the repositioning. Drop this further if play area was expanded this far below.")]
    [SerializeField] private float resetPositionThresholdY = -50f;

    private void Awake()
    {
        movementDirection = Vector3.zero;
        CheckForComponents();
    }

    // If components were added or removed during runtime (most likely by other scripts), call this again manually.
    public void CheckForComponents()
    {
        isInputMovementPresent = inputMovement != null;
        isRigidbodyPresent = mRigidbody != null;
        isCharacterControllerPresent = mCharacterController != null;
    }

    // Enable input.
    private void OnEnable()
    {
        movementDirection = Vector3.zero;
        if (!isInputMovementPresent) return;
        inputMovement.action.Enable();
    }
    // Disable input.
    private void OnDisable()
    {
        movementDirection = Vector3.zero;
        if (!isInputMovementPresent) return;
        inputMovement.action.Disable();
    }

    private void Update()
    {
        // Check player movement input, if present.
        if (isInputMovementPresent)
        {
            movementDirection = inputMovement.action.ReadValue<Vector2>();
        }
        else
        {
            movementDirection = Vector3.zero;
        }

        // Set charater movement direction based on camera's.
        characterDirection.x = movementDirection.x;
        characterDirection.y = 0f;
        characterDirection.z = movementDirection.y;
        characterDirection = Camera.main.transform.TransformDirection(characterDirection);
        characterDirection.y = 0f;
        characterDirection = movementDirection.magnitude * characterDirection.normalized;

        // Rigidbody force manipulation.
        if (isRigidbodyPresent)
        {
            switch (rigidbodySetting)
            {
                case RigidbodySetting.UseForce:
                    mRigidbody.AddForce(characterDirection * movementSpeed, ForceMode.Force);
                    break;
                case RigidbodySetting.SetVelocity:
                    mRigidbody.velocity = characterDirection * movementSpeed;
                    break;
            }

            // Reposition if under map.
            // UNTESTED
            if (resetPositionIfUnderMap &&
                transform.position.y < resetPositionThresholdY)
            {
                mRigidbody.position = resetPosition;
            }
        }

        // Character controller movement manipulation.
        // Gravity must also be accounted for.
        if (isCharacterControllerPresent)
        {
            mCharacterController.Move(movementSpeed * Time.deltaTime * characterDirection);
            mCharacterController.Move(Physics.gravity * Time.deltaTime);

            // Reposition if under map.
            if (resetPositionIfUnderMap &&
                transform.position.y < resetPositionThresholdY)
            {
                mCharacterController.enabled = false;
                transform.position = resetPosition;
                mCharacterController.enabled = true;
            }
        }
    }

    /// <summary>
    /// Returns the character's movement velocity.
    /// </summary>
    public Vector3 GetCharacterMovementDirection()
    {
        return characterDirection;
    }
}
