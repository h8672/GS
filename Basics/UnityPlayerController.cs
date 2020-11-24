using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityPlayerController : MonoBehaviour
{
    public GS.Basics.CameraMovement cameraMovement;
    public GS.Basics.CharacterMovement characterMovement;

    #region Camera Events

    private bool focusToggled = false, aimLocked = false;
    private float buttonCooldown = 0f;

    /// <summary>
    /// Updates the camera controls.
    /// Fire1 / Mouse 1: TODO, perhaps invoke attack method from Input manager.
    /// Fire2 / Mouse 2: Turns camera towards character rotation.
    /// Fire3 / Mouse 3: Toggles to turn character towards camera rotation.
    /// Fire2 and Fire3: Turns character with horizontal mouse movement.
    /// </summary>
    private void UpdateCameraControls()
    {
        if (buttonCooldown <= Time.time)
        {
            // Toggle on, turns no problem
            if (Input.GetAxis("Fire3") != 0f)
            {
                // Reset camera x rotation and turn character to camera direction.
                buttonCooldown = Time.time + .3f;
                focusToggled = !focusToggled;
                aimLocked = true;
                characterMovement.transform.LookAt(
                    (characterMovement.transform.position + (
                        cameraMovement.transform.localRotation * characterMovement.transform.forward
                    )), Vector3.up);
                cameraMovement.Target();
            }
            // Target on, turns jump a lot
            else if (!aimLocked && Input.GetAxis("Fire2") != 0f)
            {
                // Reset camera x rotation.
                focusToggled = false;
                aimLocked = true;
                cameraMovement.Target();
            }
            else if (!focusToggled && aimLocked && Input.GetAxis("Fire2") == 0f) { aimLocked = false; }
        }

        cameraMovement.Zoom(Input.GetAxis("Mouse ScrollWheel"));
        if (aimLocked)
        {
            characterMovement.Turn(Input.GetAxis("Mouse X"));
        }
        else
        {
            cameraMovement.RotateHorizontal(Input.GetAxis("Mouse X"));
            cameraMovement.RotateVertical(Input.GetAxis("Mouse Y"));
        }
    }

    #endregion // Camera Events

    #region Keyboard Events

    public bool playerTurns = true;

    private void UpdateKeyboardControls()
    {
        // Player turns or strafes from keyboard?
        // Player can only strafe if camera is locked to rotation.
        if (playerTurns && !aimLocked) { characterMovement.Turn(Input.GetAxis("Horizontal")); }
        else { characterMovement.Strafe(Input.GetAxis("Horizontal")); }

        // Character movements
        characterMovement.Move(Input.GetAxis("Vertical"));

        // NOTE: Simple example for character movement doesn't jump.
        characterMovement.Jump(Input.GetAxis("Jump"));
    }

    #endregion // Keyboard Events

    // Update is called once per frame
    void Update()
    {
        UpdateCameraControls();
        UpdateKeyboardControls();
        characterMovement.UpdateMovement();
        //Debug.Log(string.Format("<color=blue>GS.Basics.UnityPlayerController</color>: Mouse movement, x: {0}, y: {1}", Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y")), this);
    }
}
