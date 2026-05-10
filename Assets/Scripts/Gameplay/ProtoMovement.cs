using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
//Matthew
public class ProtoMovement : MonoBehaviour
{
    //references
    public InputActionAsset InputActions;
    public CinemachineCamera frontCamera;
    public GameObject[] spawnPoints;

    //movement variables
    public float speed;
    private float baseSpeed;
    public float groundedGravity = -4f;

    private InputAction moveAction;
    private InputAction sprintAction;
    public Vector2 moveInput;
    private CharacterController controller;
    private float verticalVelocity;
    public Animator animator;

    public bool isSprinting;
    
    public Animator sorcererAnimator;
    public Animator clericAnimator;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        moveAction = InputActions.FindActionMap("Player").FindAction("Move");
        sprintAction = InputActions.FindActionMap("Player").FindAction("Sprint");
        
        baseSpeed = speed;

        int spawnPosition = PlayerPrefs.GetInt("SpawnPoint", 0); //index of spawn is set from last scene, call it
        if(spawnPosition > spawnPoints.Length) //if spawn point doesnt exist, put the player at the default spawn
            PlayerPrefs.SetInt("SpawnPoint", 0);
        gameObject.transform.position = spawnPoints[spawnPosition].transform.position; //set player to spawn position


        sprintAction.performed +=
        ctx =>
        {
            isSprinting = true;
        };

        sprintAction.canceled +=
        ctx =>
        {
            isSprinting = false;
        };
    }

    private void UpdateSpeed(float newSpeed)
    {
        speed = baseSpeed * newSpeed;
        // set speed of followers
        animator.speed = newSpeed;
        if(PlayerPrefs.GetInt("PartySize", 1) > 1)
        {
            sorcererAnimator.speed = newSpeed;
            if(PlayerPrefs.GetInt("PartySize", 1) > 2)
                clericAnimator.speed = newSpeed;
        }
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

        float deadZone = 0.1f;
        Vector3 worldMove = moveDirection.normalized;

        int xDir = 0;
        int yDir = 0;

        if (worldMove.x > deadZone) xDir = 1;
        else if (worldMove.x < -deadZone) xDir = -1;
        else xDir = 0;

        if (worldMove.z > deadZone) yDir = 1;
        else if (worldMove.z < -deadZone) yDir = -1;
        else yDir = 0;

        int rotatedX = -yDir;
        int rotatedY = xDir;

        animator.SetInteger("XDirection", rotatedX);
        animator.SetInteger("YDirection", rotatedY);

        if(isSprinting)
            UpdateSpeed(1.65f);
        
        else
            UpdateSpeed(1.0f);

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

    public bool isSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name == sceneName)
                return true;
        }
        return false;
    }
}
