using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class V_GenericPlayer : MonoBehaviour
{
    [Header("Player Settings")]
    [Space(5)]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;

    [Header("Camera Settings")]
    [Space(5)]
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    [Header("Refrences")]
    [Space(5)]
    public Camera playerCamera;
    #region Refrences for scripts
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true; //stop all movement
    public bool canJump = true;
    [HideInInspector] public bool LockCursor = true;
    #endregion

    [Header("Interactions")]
    [Space(5)]
    public GameObject WaffleToSpawn;
    public Transform InteractionPoint;
    public bool IsHoldingWaffle;
    

    CharacterController characterController;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (LockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Update()
    {

        #region Handles Movment
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        #endregion

        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded && canJump)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        #endregion

        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        #endregion
    }

    public void SpawnWaffleInHand(GameObject ObjToSpawn)
    {
        Instantiate(ObjToSpawn, InteractionPoint.position, Quaternion.identity, InteractionPoint);

    }
}
