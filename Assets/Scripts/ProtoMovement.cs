using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class ProtoMovement : MonoBehaviour
{
    //references
    public InputActionAsset InputActions;
    public CinemachineCamera frontCamera;

    //movement variables
    public float speed = 12f;
    public float rotationSpeed = 10f;
    public float groundedGravity = -4f;

    private InputAction moveAction;
    private Vector2 moveInput;
    private CharacterController controller;
    private float verticalVelocity;
    private Animator animator;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        controller = GetComponent<CharacterController>();
        moveAction = InputActions.FindActionMap("Player").FindAction("Move");
        animator = this.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    private void Update()
    {
        HandleGravity();
        HandleMovement();
    }

    private void HandleMovement()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        Vector3 cameraForward = GetCameraForward();
        Vector3 cameraRight = GetCameraRight();

        Vector3 moveDirection = (cameraForward * moveInput.y + cameraRight * moveInput.x);

        //rotate character
        //if (moveDirection.magnitude > 0.1f)
        /*if (moveDirection.magnitude > 0.1f)
        {
            RotateCharacter(moveDirection);
        }*/

        animator.SetFloat("XDirection", moveInput.x);
        animator.SetFloat("YDirection", moveInput.y);

        Vector3 horizontalVelocity = moveDirection.normalized * speed;
        Vector3 finalVelocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);
        controller.Move(finalVelocity * Time.deltaTime);
    }

    private void HandleGravity()
    {
        verticalVelocity = groundedGravity;
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = frontCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = frontCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void RotateCharacter(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }

    private void OnTriggerEnter(Collider whatIHit)
    {
        if (whatIHit.tag == "EnterOverworld")
            SceneManager.LoadScene("ProtoFight");
    }
}
