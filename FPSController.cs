using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class FPSController : MonoBehaviour {
    //
    // Editor vars
    //

    [Header("Player camera")]
    public Camera playerCamera;
    public Sprite aimImage;
    public Sprite targetImage;

    public float lookSpeed = 2.0f;
    public float cameraZoomLookSpeed = 0.5f;
    public float aimSpeed = 0.75f;
    public float lookXLimit = 45.0f;

    [Header("Player attributes")]
    public float walkingSpeed = 7.5f;
    public float cameraToggleSpeed = 2f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    [Header("Options")]
    public bool canMove = true;
    public bool canSprint = true;
    public bool canCrouch = true;

    //
    // Private vars
    //

    CharacterController characterController;

    private bool cameraToggle = false;

    // Movement
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    // Camera Vars
    private float zoomFOV = 20;
    private float normalFOV = 60;
    private float zoomIncreaseRate = 175f;

    // FPS Controller
    //
    private FPSUIManager uiManager;
    private FPSFootSteps footSteps;

    void Start() {
        characterController = GetComponent<CharacterController>();
        uiManager = GetComponent<FPSUIManager>();
        footSteps = GetComponent<FPSFootSteps>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
      PlayerMove();
      ToggleCamera();
    }

    // Public Helpers
    //

    // Private Functions
    //

    private void PlayerMove() {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        // Press c to toggle crouch
        bool isCrouching = Input.GetKey(KeyCode.C);

        float curSpeedX = 0f;
        float curSpeedY = 0f;
        float playerSpeed = 0f;

        if (canMove) {
          if (isRunning) {
            playerSpeed = runningSpeed;
          } else {
            playerSpeed = walkingSpeed;
          }
            curSpeedX = playerSpeed * Input.GetAxis("Vertical");
            curSpeedY = playerSpeed * Input.GetAxis("Horizontal");
        }
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Player Jump
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded) {
            moveDirection.y = jumpSpeed;
        } else {
            moveDirection.y = movementDirectionY;
        }

        // Apply some gravity
        if (!characterController.isGrounded) {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
        if (moveDirection != Vector2.zero) {
          footSteps.PlayStepClip();
        }

        // Player and Camera rotation
        float cameraMoveSpeed = lookSpeed;
        if (cameraToggle) {
          cameraMoveSpeed = cameraZoomLookSpeed;
        }

        if (canMove) {
            rotationX += -Input.GetAxis("Mouse Y") * cameraMoveSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * cameraMoveSpeed, 0);
        }
    }

    private void ToggleCamera() {
      if(Input.GetMouseButtonDown(1)) {
        if (cameraToggle) {
          DisableInGameCamera();
        } else {
          EnableInGameCamera();
        }
      }

      // Smooth zoom
      if (cameraToggle && playerCamera.fieldOfView >= zoomFOV) {
        playerCamera.fieldOfView -= zoomIncreaseRate*Time.deltaTime;
      } else if (!cameraToggle && playerCamera.fieldOfView <= normalFOV) {
        playerCamera.fieldOfView += zoomIncreaseRate*Time.deltaTime;
      }
    }

    IEnumerator WaitToggleCameraUI(bool enabled) {
      yield return new WaitForSeconds(0.25f);
      cameraUICanvas.SetActive(enabled);
    }

    private void EnableInGameCamera() {
      StartCoroutine(WaitToggleCameraUI(true));
      cameraToggle = true;
    }

    private void DisableInGameCamera() {
      cameraUICanvas.SetActive(false); // disable right away
      cameraToggle = false;
    }
}
