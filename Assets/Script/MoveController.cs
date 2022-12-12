using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MoveController : MonoBehaviour
{
    [Header("Movimentation")]
    //Camera
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;

    //Movimentation
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] float runSpeed = 12.0f;

    //Gravity
    [SerializeField] float gravity = -13.0f;

    //Smooth walk
    [SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    //Jumping
    float jumpHeight = 3f;

    float cameraPitch = 0.0f;
    
    //Gravity
    float velocityY = 0.0f;

    CharacterController controller = null;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;
    
    public float vertical;
    public float horizontal;

    void Start()
    {
        //Movimentation
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        this.vertical = Input.GetAxis("Vertical");
        this.horizontal = Input.GetAxis("Horizontal");
        //Movimentation
        UpdateMouseLook();
        UpdateMovement();
    }

    void UpdateMouseLook() //MOUSE MOVIMENTATION
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //Smoothing
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f); //Limitating camera to 90'

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    //Movimentation
    void UpdateMovement() //KEYBOARD MOVIMENTATION 
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize(); //Normalize the vector

        //Making walk more smooth - SmoothDamp
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        //Gravity
        if (controller.isGrounded)
            velocityY = 0.0f;
        velocityY += gravity * Time.deltaTime;

        if (Input.GetButtonUp("Jump") && controller.isGrounded)
        {
            AnimationController.Instance.PlayAnimation(AnimationStates.JUMP);
            velocityY = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        };

        //Velocity formula
        if (Input.GetKey(KeyCode.LeftShift) == true || Input.GetKey(KeyCode.RightShift) == true)
        {
            AnimationController.Instance.PlayAnimation(AnimationStates.RUN);
            Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * runSpeed + Vector3.up * velocityY; 

            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY; 

            controller.Move(velocity * Time.deltaTime);   
        }
    }
}
