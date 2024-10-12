/*
 *  2D avatar script.
 *  Manipulates the Sprite Renderer and Animator components based on avatar's facing direction and movement speed
 *  versus camera's rotation.
 *  
 *  The Animator should have the following float parameters: LookDirX, LookDirY, MoveSpd
 *  These values are manipulated by this script to handle look direction and check if movement is involved per frame.
 *  
 *  Set your Blend Trees to use LookDirX and LookDirY values, and have your directional animations play based on those values.
 *  If you don't know what Blend Trees are, please look up tutorials or videos on YouTube for this.
 *  If some your sprites are only facing on one side and not the other (in this case, the right side),
 *  you can set Flip Sprite Based On Look Direction to TRUE. This will toggle Flip X on the Sprite Renderer
 *  when the avatar is facing the left side. Adjust the Flip Sprite Look X Direction Threshold value to determine
 *  the X-direction value before flipping the sprite.
 *  
 *  You can set Auto Set Look Dir Based On Position Delta to TRUE and it will do all the work for you.
 *  If you want to do it manually, set it to FALSE instead and call these methods from another script:
 *  public void SetLookDirection(Vector2 value)
 *  public void SetMovementSpeed(float speed)
 *  
 *  These methods also exist if you want to toggle look direction/move speed automation or get the look direction respectively:
 *  public void SetAutoLookState(bool state)
 *  public Vector3 GetLookDirection()
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class Avatar2D : MonoBehaviour
{
    [Header("Hover over an item to view its tooltip.")]
    [Tooltip("Set the Sprite Renderer component here.")]
    [SerializeField] private SpriteRenderer mSpriteRenderer;
    [Tooltip("If TRUE, automatically toggles Flip X boolean in Sprite Renderer based on Look Direction's X value.")]
    [SerializeField] private bool flipSpriteBasedOnLookDirection = false;
    [Tooltip("Automatically toggles Flip X boolean in Sprite Renderer if Look Direction's X value is LESS than this.\n" +
        "This means if the character is looking to the left, Flip X will become TRUE. Otherwise, FALSE.")]
    [SerializeField] private float flipSpriteLookXDirectionThreshold = -0.02f;
    private bool isSpriteRendererPresent = false;
    [Header("Animator should have these params (all float):\n" +
        "LookDirX, LookDirY, MoveSpd")]
    [Tooltip("Set the Animator component here.")]
    [SerializeField] private Animator mAnimator;
    private bool isAnimatorPresent = false;
    [Tooltip("If TRUE, enables setting look direction and movement speed by this script if the position is being changed by another.\n" +
        "Call SetAutoLookState to change this during runtime.")]
    [SerializeField] private bool autoSetLookDirBasedOnPositionDelta = true;
    [Tooltip("Distance threshold for the automatic look direction and movement speed setting.")]
    [SerializeField] private float autoSetLookDirDistThreshold = 0.0001f;

    private Vector3 lookDirection = Vector3.forward;
    private Vector3 lookDirectionCameraBased = Vector3.forward;
    private float movementSpeed = 0f;
    private Vector3 lastPosition = Vector3.zero;
    private Vector3 positionDelta = Vector3.zero;

    private void Awake()
    {
        CheckForComponents();
    }

    private void OnEnable()
    {
        lastPosition = transform.position;
    }

    // If components were added or removed during runtime (most likely by other scripts), call this again manually.
    public void CheckForComponents()
    {
        if (mAnimator == null) mAnimator = GetComponent<Animator>();
        if (mSpriteRenderer == null) mSpriteRenderer = GetComponent<SpriteRenderer>();
        isAnimatorPresent = mAnimator != null;
        isSpriteRendererPresent = mSpriteRenderer != null;
    }

    private void Update()
    {
        // Is the boolean true for automatic look direction and movement speed value setting?
        if (autoSetLookDirBasedOnPositionDelta)
        {
            // Get and set position delta
            positionDelta = transform.position - lastPosition;
            movementSpeed = 0f;
            // Is the delta past the threshold?
            if (positionDelta.magnitude > autoSetLookDirDistThreshold)
            {
                // Set delta as the look direction
                lookDirection = positionDelta.normalized;
                // Record current position
                lastPosition = transform.position;
                // Movement speed adjustment
                movementSpeed = 1f;
            }
        }

        // Get look direction based on camera direction.
        lookDirectionCameraBased = Camera.main.transform.InverseTransformDirection(lookDirection);
        lookDirectionCameraBased.y = 0f;
        lookDirectionCameraBased.Normalize();
        if (isAnimatorPresent)
        {
            // Apply calculated look direction to sprite animator values.
            mAnimator.SetFloat("LookDirX", lookDirectionCameraBased.x);
            mAnimator.SetFloat("LookDirY", lookDirectionCameraBased.z);
            mAnimator.SetFloat("MoveSpd", movementSpeed);
        }
        if (isSpriteRendererPresent &&
            flipSpriteBasedOnLookDirection)
        {
            // Sprites can't flip in the animator, but the Sprite Renderer component is able to flip the sprite.
            // If the character is looking to the left, it will flip the right-facing sprites.
            // Be sure all sprites are facing rightwards with up to 90-degree variation in either direction.
            mSpriteRenderer.flipX = lookDirectionCameraBased.x < flipSpriteLookXDirectionThreshold;
        }
    }

    public void SetLookDirection(Vector2 value)
    {
        if (value.magnitude < 0.01f) return;
        lookDirection = value.normalized;
        lookDirection.z = lookDirection.y;
        lookDirection.y = 0f;
    }
    public void SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }
    public void SetAutoLookState(bool state)
    {
        autoSetLookDirBasedOnPositionDelta = state;
    }
    public Vector3 GetLookDirection()
    {
        return lookDirection;
    }
}